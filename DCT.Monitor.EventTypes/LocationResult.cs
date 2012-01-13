using System;
using DCT.ObjectModel;

namespace DCT.Monitor.Entities
{
    [Serializable]
    public class LocationResult: NotifyObject
    {
        public double Latitude { get; set; }
        private double _latitude { get { return Latitude; } set { SendPropertyChanged(() => this.Latitude); } }

        public double Longitude { get; set; }
        private double _longitude { get { return Longitude; } set { SendPropertyChanged(() => this.Longitude); } }

        public int Count { get; set; }
        private int _count { get { return Count; } set { SendPropertyChanged(() => this.Count); } }
    }
}
