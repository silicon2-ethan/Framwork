using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Linq.Expressions;
using System.ComponentModel;

namespace SL.Framework.Dynamic
{
    /// <summary>
    /// See http://amazedsaint.blogspot.com/2010/02/introducing-elasticobject-for-net-40.html for details
    /// </summary>
    public class ElasticObject : DynamicObject, IElasticHierarchyWrapper, INotifyPropertyChanged
    {
        #region Private

        private readonly IElasticHierarchyWrapper _elasticProvider = new SimpleHierarchyWrapper();
        private NodeType _nodeType = NodeType.Element;

        #endregion

        #region Ctor

        public ElasticObject()
        {
            InternalName = "id" + Guid.NewGuid();
        }

        public ElasticObject(string name)
        {
            InternalName = name;
        }

        internal ElasticObject(string name, object value)
            : this(name)
        {
            InternalValue = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a member to this element, with the specified value
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal ElasticObject CreateOrGetAttribute(string memberName, object value)
        {
            if (!HasAttribute(memberName))
            {
                AddAttribute(memberName, new ElasticObject(memberName, value));
            }

            return Attribute(memberName);
        }

        /// <summary>
        /// Fully qualified name
        /// </summary>
        public string InternalFullName
        {
            get
            {
                var path = InternalName;
                var parent = InternalParent;

                while (parent != null)
                {
                    path = parent.InternalName + "_" + path;
                    parent = parent.InternalParent;
                }

                return path;
            }
        }

        /// <summary>
        /// Interpret a method call
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var obj = new ElasticObject(binder.Name, null);
            AddElement(obj);
            result = obj;
            return true;
        }

        /// <summary>
        /// Interpret the invocation of a binary operation
        /// </summary>
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            if (binder.Operation == ExpressionType.LeftShiftAssign && _nodeType == NodeType.Element)
            {
                InternalContent = arg;
                result = this;
                return true;
            }
            else if (binder.Operation == ExpressionType.LeftShiftAssign && _nodeType == NodeType.Attribute)
            {
                InternalValue = arg;
                result = this;
                return true;
            }
            else if (binder.Operation == ExpressionType.LeftShift)
            {
                if (arg is string)
                {
                    var exp = new ElasticObject(arg as string, null) {_nodeType = NodeType.Element};
                    AddElement(exp);
                    result = exp;
                    return true;
                }
                else if (arg is ElasticObject)
                {
                    var eobj = arg as ElasticObject;
                    if (!Elements.Contains(eobj))
                        AddElement(eobj);
                    result = eobj;
                    return true;
                }
            }
            else if (binder.Operation == ExpressionType.LessThan)
            {
                var memberName = arg as string;
                if (arg is string)
                {
                    if (!HasAttribute(memberName))
                    {
                        var att = new ElasticObject(memberName, null);
                        AddAttribute(memberName, att);
                        result = att;
                        return true;
                    }
                    else
                    {
                        throw new InvalidOperationException("An attribute with name" + memberName + " already exists");
                    }
                }
                else if (arg is ElasticObject)
                {
                    var eobj = arg as ElasticObject;
                    AddAttribute(memberName, eobj);
                    result = eobj;
                    return true;
                }
            }
            else if (binder.Operation == ExpressionType.GreaterThan)
            {
                if (arg is FormatType)
                {
                    result = this.ToXElement();
                    return true;
                }
            }
            return base.TryBinaryOperation(binder, arg, out result);
        }

        /// <summary>
        /// Try the unary operation.
        /// </summary>
        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            if (binder.Operation == ExpressionType.OnesComplement)
            {
                //TODO: 수정 CDATA tag 제외 출력하도록 설정 (2015.01.16 / jeongyong jo)
                //result = (nodeType == NodeType.Element) ? InternalContent : InternalValue;
                result = InternalValue;
                return true;
            }

            return base.TryUnaryOperation(binder, out result);
        }

        /// <summary>
        /// Handle the indexer operations
        /// </summary>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = null;

            if ((indexes.Length == 1) && indexes[0] == null)
            {
                result = _elasticProvider.Elements.ToList();
            }
            else if ((indexes.Length == 1) && indexes[0] is int)
            {

                var indx = (int) indexes[0];
                var elmt = Elements.ElementAt(indx);
                result = elmt;

            }
            else if ((indexes.Length == 1) && indexes[0] is Func<dynamic, bool>)
            {
                var filter = indexes[0] as Func<dynamic, bool>;
                result = Elements.Where
                    (c => filter(c)).ToList();
            }
            else
            {
                result = Elements.Where
                    (c => indexes.Cast<string>().Contains(c.InternalName)).ToList();
            }

            return true;
        }

        /// <summary>
        /// Catch a get member invocation
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_elasticProvider.HasAttribute(binder.Name))
            {
                result = _elasticProvider.Attribute(binder.Name).InternalValue;
            }
            else
            {
                var obj = _elasticProvider.Element(binder.Name);
                if (obj != null)
                {
                    result = obj;
                }
                else
                {
                    var exp = new ElasticObject(binder.Name, null);
                    _elasticProvider.AddElement(exp);
                    result = exp;
                }
            }

            return true;
        }

        /// <summary>
        /// Catch a set member invocation
        /// </summary>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var memberName = binder.Name;

            if (value is ElasticObject)
            {
                var eobj = value as ElasticObject;
                if (!Elements.Contains(eobj))
                    AddElement(eobj);
            }
            else
            {
                if (!_elasticProvider.HasAttribute(memberName))
                {
                    _elasticProvider.AddAttribute(memberName, new ElasticObject(memberName, value));
                }
                else
                {
                    _elasticProvider.SetAttributeValue(memberName, value);
                }
            }

            OnPropertyChanged(memberName);

            return true;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        #region IElasticHierarchyWrapper<ElasticObject> Members

        public IEnumerable<KeyValuePair<string, ElasticObject>> Attributes
        {
            get { return _elasticProvider.Attributes; }
        }

        public bool HasAttribute(string name)
        {
            return _elasticProvider.HasAttribute(name);
        }

        public IEnumerable<ElasticObject> Elements
        {
            get { return _elasticProvider.Elements; }
        }

        public void SetAttributeValue(string name, object obj)
        {
            _elasticProvider.SetAttributeValue(name, obj);
        }

        public object GetAttributeValue(string name)
        {
            return _elasticProvider.GetAttributeValue(name);
        }

        public ElasticObject Attribute(string name)
        {
            return _elasticProvider.Attribute(name);
        }

        public ElasticObject Element(string name)
        {
            return _elasticProvider.Element(name);
        }

        public void AddAttribute(string key, ElasticObject value)
        {
            value._nodeType = NodeType.Attribute;
            value.InternalParent = this;
            _elasticProvider.AddAttribute(key, value);
        }

        public void RemoveAttribute(string key)
        {
            _elasticProvider.RemoveAttribute(key);
        }

        public void AddElement(ElasticObject element)
        {
            element._nodeType = NodeType.Element;
            element.InternalParent = this;
            _elasticProvider.AddElement(element);
        }

        public void RemoveElement(ElasticObject element)
        {
            _elasticProvider.RemoveElement(element);
        }

        public object InternalValue
        {
            get { return _elasticProvider.InternalValue; }
            set { _elasticProvider.InternalValue = value; }
        }

        public string EncodeQuotation
        {
            get { return _elasticProvider.InternalValue.ToString().Replace("'", "\\'").Replace("\"", "\\\""); }
        }

        public object InternalContent
        {
            get { return _elasticProvider.InternalContent; }
            set { _elasticProvider.InternalContent = value; }
        }

        public string InternalName
        {
            get { return _elasticProvider.InternalName; }
            set { _elasticProvider.InternalName = value; }
        }

        public ElasticObject InternalParent
        {
            get { return _elasticProvider.InternalParent; }
            set { _elasticProvider.InternalParent = value; }
        }

        #endregion
    }

    public enum NodeType
    {
        Element,
        Attribute
    }

    public enum FormatType
    {
        Xml,
        Json
    }
}