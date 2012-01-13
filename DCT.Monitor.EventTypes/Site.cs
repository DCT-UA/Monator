using System;
using DCT.ObjectModel;

namespace DCT.Monitor.Entities
{
	[Serializable]
    public class Site: NotifyObject, IEntity<Guid>, IPageSelector
    {
        private Guid _id;
        public Guid Id { get { return _id; } set { _id = value; SendPropertyChanged(() => this.Id); } }

        private Guid _userId;
        public Guid UserId { get { return _userId; } set { _userId = value; SendPropertyChanged(() => this.UserId); } }

        private string _domain;
        public string Domain { get { return _domain; } set { _domain = value; SendPropertyChanged(() => this.Domain); SendPropertyChanged("Pattern"); } }

		public bool ContainsSubdomains { get; set; }

        public string Pattern
        {
            get { return Domain; }
            set { throw new NotSupportedException(); }
        }

        public Guid? ParentId
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }

        public int SelectorType
        {
            get { return (int)PageSelectorType.DomainPattern; }
            set { throw new NotSupportedException(); }
        }
    }
}
