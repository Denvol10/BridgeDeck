using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BridgeDeck.Infrastructure;
using BridgeDeck.Models;

namespace BridgeDeck.ViewModels
{
    internal class MainWindowViewModel : Base.ViewModel
    {
        private RevitModelForfard _revitModel;

        internal RevitModelForfard RevitModel
        {
            get => _revitModel;
            set => _revitModel = value;
        }


        #region Заголовок

        private string _title = "Bridge Deck";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion


        #region Команды



        #endregion


        #region Конструктор класса MainWindowViewModel
        public MainWindowViewModel()
        {
            #region Команды


            #endregion
        }
        #endregion
    }
}
