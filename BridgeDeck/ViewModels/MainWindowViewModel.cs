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

        #region Элементы оси трассы

        private string _roadAxisElemIds;

        public string RoadAxisElemIds
        {
            get => _roadAxisElemIds;
            set => Set(ref _roadAxisElemIds, value);
        }

        #endregion

        #region Команды

        #region Получение оси трассы

        public ICommand GetRoadAxis { get; }

        private void OnGetRoadAxisCommandExecuted(object parameter)
        {
            RevitCommand.mainView.Hide();
            RevitModel.GetPolyCurve();
            RoadAxisElemIds = RevitModel.RoadAxisElemIds;
            RevitCommand.mainView.ShowDialog();
        }

        private bool CanGetRoadAxisCommandExecute(object parameter)
        {
            return true;
        }

        #endregion


        #endregion


        #region Конструктор класса MainWindowViewModel
        public MainWindowViewModel()
        {
            #region Команды

            GetRoadAxis = new LambdaCommand(OnGetRoadAxisCommandExecuted, CanGetRoadAxisCommandExecute);

            #endregion
        }
        #endregion
    }
}
