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
using System.Windows;

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

        #region Элементы линии на поверхности 1
        private string _roadLineElemIds1;

        public string RoadLineElemIds1
        {
            get => _roadLineElemIds1;
            set => Set(ref _roadLineElemIds1, value);
        }
        #endregion

        #region Элементы линии на поверхности 2
        private string _roadLineElemIds2;

        public string RoadLineElemIds2
        {
            get => _roadLineElemIds2;
            set => Set(ref _roadLineElemIds2, value);
        }
        #endregion

        #region Элемент границы 1
        private string _boundCurveId1;
        public string BoundCurveId1
        {
            get => _boundCurveId1;
            set => Set(ref _boundCurveId1, value);
        }
        #endregion

        #region Элемент границы 2
        private string _boundCurveId2;
        public string BoundCurveId2
        {
            get => _boundCurveId2;
            set => Set(ref _boundCurveId2, value);
        }
        #endregion

        #region Список семейств и их типоразмеров
        private ObservableCollection<string> _genericModelFamilySymbols = new ObservableCollection<string>();
        public ObservableCollection<string> GenericModelFamilySymbols
        {
            get => _genericModelFamilySymbols;
            set => Set(ref _genericModelFamilySymbols, value);
        }
        #endregion

        #region Выбранный типоразмер семейства
        private string _familySymbolName;
        public string FamilySymbolName
        {
            get => _familySymbolName;
            set => Set(ref _familySymbolName, value);
        }
        #endregion

        #region Количество точек ручек формы
        private int _countShapeHandlePoints = 1;
        public int CountShapeHandlePoints
        {
            get => _countShapeHandlePoints;
            set => Set(ref _countShapeHandlePoints, value);
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

        #region Получение линии на поверхности дороги 1
        public ICommand GetRoadLines1 { get; }

        private void OnGetRoadLines1CommandExecuted(object parameter)
        {
            RevitCommand.mainView.Hide();
            RevitModel.GetRoadLine1();
            RoadLineElemIds1 = RevitModel.RoadLineElemIds1;
            RevitCommand.mainView.ShowDialog();
        }

        private bool CanGetRoadLines1CommandExecute(object parameter)
        {
            return true;
        }
        #endregion

        #region Получение линии на поверхности дороги 2
        public ICommand GetRoadLines2 { get; }

        private void OnGetRoadLines2CommandExecuted(object parameter)
        {
            RevitCommand.mainView.Hide();
            RevitModel.GetRoadLine2();
            RoadLineElemIds2 = RevitModel.RoadLineElemIds2;
            RevitCommand.mainView.ShowDialog();
        }

        private bool CanGetRoadLines2CommandExecute(object parameter)
        {
            return true;
        }
        #endregion

        #region Получение границы плиты 1
        public ICommand GetBoundCurve1 { get; }

        private void OnGetBoundCurve1CommandExecuted(object parameter)
        {
            RevitCommand.mainView.Hide();
            RevitModel.GetBoundCurve1();
            BoundCurveId1 = RevitModel.BoundCurveId1;
            RevitCommand.mainView.ShowDialog();
        }

        public bool CanGetBoundCurve1CommandExecute(object parameter)
        {
            return true;
        }
        #endregion

        #region Получение границы плиты 2
        public ICommand GetBoundCurve2 { get; }

        private void OnGetBoundCurve2CommandExecuted(object parameter)
        {
            RevitCommand.mainView.Hide();
            RevitModel.GetBoundCurve2();
            BoundCurveId2 = RevitModel.BoundCurveId2;
            RevitCommand.mainView.ShowDialog();
        }

        private bool CanGetBoundCurve2CommandExecute(object parameter)
        {
            return true;
        }
        #endregion

        #region Создать экземпляры в модели
        public ICommand CreateAdaptiveFamilyInstances { get; }

        private void OnCreateAdaptiveFamilyInstancesCommandExecuted(object parameter)
        {
            RevitModel.CreateAdaptivePointsFamilyInstanse(FamilySymbolName, CountShapeHandlePoints);
            RevitCommand.mainView.Close();
        }

        private bool CanCreateAdaptiveFamilyInstancesCommandExecute(object parameter)
        {
            return true;
        }
        #endregion

        #endregion

        #region Конструктор класса MainWindowViewModel
        public MainWindowViewModel(RevitModelForfard revitModel)
        {
            RevitModel = revitModel;

            GenericModelFamilySymbols = RevitModel.GetFamilySymbolNames();

            #region Команды
            GetRoadAxis = new LambdaCommand(OnGetRoadAxisCommandExecuted, CanGetRoadAxisCommandExecute);

            GetRoadLines1 = new LambdaCommand(OnGetRoadLines1CommandExecuted, CanGetRoadLines1CommandExecute);

            GetRoadLines2 = new LambdaCommand(OnGetRoadLines2CommandExecuted, CanGetRoadLines2CommandExecute);

            GetBoundCurve1 = new LambdaCommand(OnGetBoundCurve1CommandExecuted, CanGetBoundCurve1CommandExecute);

            GetBoundCurve2 = new LambdaCommand(OnGetBoundCurve2CommandExecuted, CanGetBoundCurve2CommandExecute);

            CreateAdaptiveFamilyInstances = new LambdaCommand(OnCreateAdaptiveFamilyInstancesCommandExecuted,
                                                              CanCreateAdaptiveFamilyInstancesCommandExecute);
            #endregion
        }

        public MainWindowViewModel()
        { }
        #endregion
    }
}
