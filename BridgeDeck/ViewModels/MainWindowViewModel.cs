﻿using System.Collections.ObjectModel;
using System.Linq;
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

        private int _familySymbolIndex = (int)Properties.Settings.Default["FamilySymbolIndex"];

        #region Заголовок
        private string _title = "Плита ПС";

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
        private ObservableCollection<FamilySymbolSelector> _genericModelFamilySymbols = new ObservableCollection<FamilySymbolSelector>();
        public ObservableCollection<FamilySymbolSelector> GenericModelFamilySymbols
        {
            get => _genericModelFamilySymbols;
            set => Set(ref _genericModelFamilySymbols, value);
        }
        #endregion

        #region Выбранный типоразмер семейства
        private FamilySymbolSelector _familySymbolName;
        public FamilySymbolSelector FamilySymbolName
        {
            get => _familySymbolName;
            set => Set(ref _familySymbolName, value);
        }
        #endregion

        #region Количество точек ручек формы
        private int _countShapeHandlePoints;
        public int CountShapeHandlePoints
        {
            get => _countShapeHandlePoints;
            set => Set(ref _countShapeHandlePoints, value);
        }
        #endregion

        #region Повернуть профиль
        private bool _isRotate = (bool)Properties.Settings.Default["IsRotate"];
        public bool IsRotate
        {
            get => _isRotate;
            set => Set(ref _isRotate, value);
        }
        #endregion

        #region Профиль вертикально
        private bool _isVertical = (bool)Properties.Settings.Default["IsVertical"];
        public bool IsVertical
        {
            get => _isVertical;
            set => Set(ref _isVertical, value);
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
            CountShapeHandlePoints = RevitModel.GetCountShapeHandlePoints(FamilySymbolName);
            RevitModel.CreateAdaptivePointsFamilyInstanse(FamilySymbolName, CountShapeHandlePoints, IsRotate, IsVertical);
            SaveSettings();
            RevitCommand.mainView.Close();
        }

        private bool CanCreateAdaptiveFamilyInstancesCommandExecute(object parameter)
        {
            return true;
        }
        #endregion

        #region Закрыть окно
        public ICommand CloseWindow { get; }

        private void OnCloseWindowCommandExecuted(object parameter)
        {
            SaveSettings();
            RevitCommand.mainView.Close();
        }

        private bool CanCloseWindowCommandExecute(object parameter)
        {
            return true;
        }
        #endregion

        #endregion

        private void SaveSettings()
        {
            Properties.Settings.Default["RoadAxisElemIds"] = RoadAxisElemIds;
            Properties.Settings.Default["RoadLineElemIds1"] = RoadLineElemIds1;
            Properties.Settings.Default["RoadLineElemIds2"] = RoadLineElemIds2;
            Properties.Settings.Default["BoundCurveId1"] = BoundCurveId1;
            Properties.Settings.Default["BoundCurveId2"] = BoundCurveId2;
            Properties.Settings.Default["FamilySymbolIndex"] = GenericModelFamilySymbols.IndexOf(FamilySymbolName);
            Properties.Settings.Default["IsRotate"] = IsRotate;
            Properties.Settings.Default["IsVertical"] = IsVertical;
            Properties.Settings.Default.Save();
        }

        #region Конструктор класса MainWindowViewModel
        public MainWindowViewModel(RevitModelForfard revitModel)
        {
            RevitModel = revitModel;

            GenericModelFamilySymbols = RevitModel.GetFamilySymbolNames();

            #region Инициализация значения элементам оси из Settings
            if (!(Properties.Settings.Default["RoadAxisElemIds"] is null))
            {
                string axisElementIdInSettings = Properties.Settings.Default["RoadAxisElemIds"].ToString();
                if(RevitModel.IsLinesExistInModel(axisElementIdInSettings) && !string.IsNullOrEmpty(axisElementIdInSettings))
                {
                    RoadAxisElemIds = axisElementIdInSettings;
                    RevitModel.GetAxisBySettings(axisElementIdInSettings);
                }
            }
            #endregion

            #region Инициализация значения элементам линии на поверхности 1
            if (!(Properties.Settings.Default["RoadLineElemIds1"] is null))
            {
                string line1ElementIdInSettings = Properties.Settings.Default["RoadLineElemIds1"].ToString();
                if(RevitModel.IsLinesExistInModel(line1ElementIdInSettings) && !string.IsNullOrEmpty(line1ElementIdInSettings))
                {
                    RoadLineElemIds1 = line1ElementIdInSettings;
                    RevitModel.GetRoadLines1BySettings(line1ElementIdInSettings);
                }
            }
            #endregion

            #region Инициализация значения элементам линии на поверхности 2
            if (!(Properties.Settings.Default["RoadLineElemIds2"] is null))
            {
                string line2ElementIdInSettings = Properties.Settings.Default["RoadLineElemIds2"].ToString();
                if(RevitModel.IsLinesExistInModel(line2ElementIdInSettings) && !string.IsNullOrEmpty(line2ElementIdInSettings))
                {
                    RoadLineElemIds2 = line2ElementIdInSettings;
                    RevitModel.GetRoadLines2BySettings(line2ElementIdInSettings);
                }
            }
            #endregion

            #region Инициализация значения элемента граница плиты 1
            if (!(Properties.Settings.Default["BoundCurveId1"] is null))
            {
                string bound1ElementIdSettings = Properties.Settings.Default["BoundCurveId1"].ToString();
                if(RevitModel.IsBoundLineExistInModel(bound1ElementIdSettings) && !string.IsNullOrEmpty(bound1ElementIdSettings))
                {
                    BoundCurveId1 = bound1ElementIdSettings;
                    RevitModel.GetBound1BySettings(bound1ElementIdSettings);
                }
            }
            #endregion

            #region Инициализация значения элемента граница плиты 2
            if (!(Properties.Settings.Default["BoundCurveId2"] is null))
            {
                string bound2ElementIdSetiings = Properties.Settings.Default["BoundCurveId2"].ToString();
                if(RevitModel.IsBoundLineExistInModel(bound2ElementIdSetiings) && !string.IsNullOrEmpty(bound2ElementIdSetiings))
                {
                    BoundCurveId2 = bound2ElementIdSetiings;
                    RevitModel.GetBound2BySettings(bound2ElementIdSetiings);
                }
            }
            #endregion

            #region Инициализация значения типоразмера семейства
            if(_familySymbolIndex >= 0 && _familySymbolIndex <= GenericModelFamilySymbols.Count - 1)
            {
                FamilySymbolName = GenericModelFamilySymbols.ElementAt(_familySymbolIndex);
            }
            #endregion

            #region Команды
            GetRoadAxis = new LambdaCommand(OnGetRoadAxisCommandExecuted, CanGetRoadAxisCommandExecute);

            GetRoadLines1 = new LambdaCommand(OnGetRoadLines1CommandExecuted, CanGetRoadLines1CommandExecute);

            GetRoadLines2 = new LambdaCommand(OnGetRoadLines2CommandExecuted, CanGetRoadLines2CommandExecute);

            GetBoundCurve1 = new LambdaCommand(OnGetBoundCurve1CommandExecuted, CanGetBoundCurve1CommandExecute);

            GetBoundCurve2 = new LambdaCommand(OnGetBoundCurve2CommandExecuted, CanGetBoundCurve2CommandExecute);

            CreateAdaptiveFamilyInstances = new LambdaCommand(OnCreateAdaptiveFamilyInstancesCommandExecuted,
                                                              CanCreateAdaptiveFamilyInstancesCommandExecute);

            CloseWindow = new LambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
            #endregion
        }

        public MainWindowViewModel()
        { }
        #endregion
    }
}
