using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Library;
using Library.Collections;
using Library.Io;

namespace Amoeba.Windows
{
    [DataContract(Name = "StoreCategorizeTreeItem", Namespace = "http://Amoeba/Windows")]
    class StoreCategorizeTreeItem : ICloneable<StoreCategorizeTreeItem>, IThisLock
    {
        private string _name;
        private LockedList<StoreTreeItem> _storeTreeItems;
        private LockedList<StoreCategorizeTreeItem> _children;
        private bool _isExpanded = true;

        private static readonly object _initializeLock = new object();
        private volatile object _thisLock;

        [DataMember(Name = "Name")]
        public string Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return _name;
                }
            }
            set
            {
                lock (this.ThisLock)
                {
                    _name = value;
                }
            }
        }

        [DataMember(Name = "StoreTreeItems")]
        public LockedList<StoreTreeItem> StoreTreeItems
        {
            get
            {
                lock (this.ThisLock)
                {
                    if (_storeTreeItems == null)
                        _storeTreeItems = new LockedList<StoreTreeItem>();

                    return _storeTreeItems;
                }
            }
        }

        [DataMember(Name = "Children")]
        public LockedList<StoreCategorizeTreeItem> Children
        {
            get
            {
                lock (this.ThisLock)
                {
                    if (_children == null)
                        _children = new LockedList<StoreCategorizeTreeItem>();

                    return _children;
                }
            }
        }

        [DataMember(Name = "IsExpanded")]
        public bool IsExpanded
        {
            get
            {
                lock (this.ThisLock)
                {
                    return _isExpanded;
                }
            }
            set
            {
                lock (this.ThisLock)
                {
                    _isExpanded = value;
                }
            }
        }

        #region ICloneable<StoreCategorizeTreeItem>

        public StoreCategorizeTreeItem Clone()
        {
            lock (this.ThisLock)
            {
                var ds = new DataContractSerializer(typeof(StoreCategorizeTreeItem));

                using (BufferStream stream = new BufferStream(BufferManager.Instance))
                {
                    using (WrapperStream wrapperStream = new WrapperStream(stream, true))
                    using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(wrapperStream))
                    {
                        ds.WriteObject(xmlDictionaryWriter, this);
                    }

                    stream.Position = 0;

                    using (XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
                    {
                        return (StoreCategorizeTreeItem)ds.ReadObject(xmlDictionaryReader);
                    }
                }
            }
        }

        #endregion

        #region IThisLock

        public object ThisLock
        {
            get
            {
                if (_thisLock == null)
                {
                    lock (_initializeLock)
                    {
                        if (_thisLock == null)
                        {
                            _thisLock = new object();
                        }
                    }
                }

                return _thisLock;
            }
        }

        #endregion
    }
}
