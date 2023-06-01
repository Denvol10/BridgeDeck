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
using BridgeDeck.Models;
using BridgeDeck.ViewModels;
using BridgeDeck.Infrastructure;
using System.Windows.Controls;

namespace BridgeDeck
{
    public class RevitModelForfard
    {
        private UIApplication Uiapp { get; set; } = null;
        private Application App { get; set; } = null;
        private UIDocument Uidoc { get; set; } = null;
        private Document Doc { get; set; } = null;

        public RevitModelForfard(UIApplication uiapp)
        {
            Uiapp = uiapp;
            App = uiapp.Application;
            Uidoc = uiapp.ActiveUIDocument;
            Doc = uiapp.ActiveUIDocument.Document;
        }

        #region Ось трассы
        public PolyCurve RoadAxis { get; set; }

        private string _roadAxisElemIds;
        public string RoadAxisElemIds
        {
            get => _roadAxisElemIds;
            set => _roadAxisElemIds = value;
        }

        public void GetPolyCurve()
        {
            var curves = RevitGeometryUtils.GetCurvesByRectangle(Uiapp, out _roadAxisElemIds);
            RoadAxis = new PolyCurve(curves);
        }
        #endregion

        #region Линия на поверхности 1
        public List<Line> RoadLines1 { get; set; }

        private string _roadLineElemIds1;
        public string RoadLineElemIds1
        {
            get => _roadLineElemIds1;
            set => _roadLineElemIds1 = value;
        }

        public void GetRoadLine1()
        {
            RoadLines1 = RevitGeometryUtils.GetRoadLines(Uiapp, out _roadLineElemIds1);
        }
        #endregion

        #region Линия на поверхности 2
        public List<Line> RoadLines2 { get; set; }

        private string _roadLineElemIds2;
        public string RoadLineElemIds2
        {
            get => _roadLineElemIds2;
            set => _roadLineElemIds2 = value;
        }

        public void GetRoadLine2()
        {
            RoadLines2 = RevitGeometryUtils.GetRoadLines(Uiapp, out _roadLineElemIds2);
        }
        #endregion

        #region Граница плиты 1
        public Curve BoundCurve1 { get; set; }

        private string _boundCurveId1;
        public string BoundCurveId1
        {
            get => _boundCurveId1;
            set => _boundCurveId1 = value;
        }

        public void GetBoundCurve1()
        {
            BoundCurve1 = RevitGeometryUtils.GetBoundCurve(Uiapp, out _boundCurveId1);
        }
        #endregion

        #region Граница плиты 2
        public Curve BoundCurve2 { get; set; }

        private string _boundCurveId2;
        public string BoundCurveId2
        {
            get => _boundCurveId2;
            set => _boundCurveId2 = value;
        }
        
        public void GetBoundCurve2()
        {
            BoundCurve2 = RevitGeometryUtils.GetBoundCurve(Uiapp, out _boundCurveId2);
        }
        #endregion

        #region Список названий типоразмеров семейств
        public ObservableCollection<string> GetFamilySymbolNames()
        {
            var familySymbolNames = new ObservableCollection<string>();
            var allFamilies = new FilteredElementCollector(Doc).OfClass(typeof(Family)).OfType<Family>();
            var genericModelFamilies = allFamilies.Where(f => f.FamilyCategory.Id.IntegerValue == (int)BuiltInCategory.OST_GenericModel);
            if (genericModelFamilies.Count() == 0)
                return familySymbolNames;

            foreach (var family in genericModelFamilies)
            {
                foreach(var symbolId in family.GetFamilySymbolIds())
                {
                    var familySymbol = Doc.GetElement(symbolId);
                    familySymbolNames.Add($"{family.Name}-{familySymbol.Name}");
                }
            }

            return familySymbolNames;
        }
        #endregion

        #region Получение типоразмера по имени
        public FamilySymbol GetFamilySymbolByName(string familyAndSymbolName)
        {
            var familyName = familyAndSymbolName.Split('-').First();
            var symbolName = familyAndSymbolName.Split('-').Last();

            Family family = new FilteredElementCollector(Doc).OfClass(typeof(Family)).Where(f => f.Name == familyName).First() as Family;
            var symbolIds = family.GetFamilySymbolIds();
            foreach (var symbolId in symbolIds)
            {
                FamilySymbol fSymbol = (FamilySymbol)Doc.GetElement(symbolId);
                if (fSymbol.get_Parameter(BuiltInParameter.SYMBOL_NAME_PARAM).AsString() == symbolName)
                {
                    return fSymbol;
                }
            }
            return null;
        }
        #endregion
    }
}
