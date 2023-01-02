using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batch_Rename
{
    public class FileName : INotifyPropertyChanged
    {
        private string _shortname;
        private string _newname;
        private string _fullpath;
        private string _error;

        public string ShortName
        {
            get => _shortname; set
            {
                _shortname = value;
                NotifyChanged("Filename");
            }
        }

        public string NewName
        {
            get => _newname;
            set
            {
                _newname = value;
                NotifyChanged("Newfilename");
            }
        }

        public string FullPath
        {
            get => _fullpath;
            set
            {
                _fullpath = value;
                NotifyChanged("Path");
            }
        }

        public string Error
        {
            get => _error;
            set
            {
                _error = value;
                NotifyChanged("Erro");
            }
        }

        private void NotifyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(v));
            }
        }

        public FileName Clone()
        {
            return new FileName()
            {
                ShortName = this._shortname,
                NewName = this._newname,
                FullPath = this._fullpath,
                Error = this._error
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
