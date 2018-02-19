using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MobileQALogChecker.DataTools
{
    /// <summary>
    /// This class was created for future work that may alllow more automated checking of all the fields to determine if their values are correct.
    /// </summary>

    public class FieldDataViewModel
    {
        public ObservableCollection<Field> Fields { get; set; }
        public FieldDataViewModel()
        {
            
            Fields = new ObservableCollection<Field>()
            {
                new Field("stuff", true)
            };          
        }
        
    }

    public class Field : INotifyPropertyChanged
    {
        
        public Field(string field, bool toValidate)
        {
            _field = field;
            _toValidate = toValidate;
        }
        private string _field {get; set;}
        private bool _toValidate {get; set;}
        
        public string Name
        {
            get { return _field; }
            private set { }
        }
        public bool ToValidate
        {
            get { return _toValidate; }
            set
            {
                _toValidate = value;
                NotifyPropertyChanged("ToValidate");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string strPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        }
    }

}

