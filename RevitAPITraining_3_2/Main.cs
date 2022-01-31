using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITraining_3_2
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
           
            IList<Reference> reference = uidoc.Selection.PickObjects(ObjectType.Element, "Выберите трубы");
            List<ElementId> pipes = new List<ElementId>();

            double Sum = 0;

            foreach (var item in reference)
            {
                Element element = doc.GetElement(item);
                if (element is Pipe)
                {
                    Pipe pipe = element as Pipe;
                    if (!pipes.Contains(pipe.Id))
                    {
                        pipes.Add(pipe.Id);
                        double Sum1 = pipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                        double Sum2 = UnitUtils.ConvertFromInternalUnits(Sum1, UnitTypeId.Meters);

                        Sum += Sum2;
                    }
                }
            }
            TaskDialog.Show("Объем стен", $"{Sum.ToString()} m");
            return Result.Succeeded;
        }
    }
}
