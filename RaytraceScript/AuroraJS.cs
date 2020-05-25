using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora;
using Microsoft.ClearScript.V8;

namespace RaytraceScript
{
    class AuroraJS
    {
        static Point3 JsPoint(double x, double y, double z) => new Point3(x, y, z);
        static Vector3 JsVector(double x, double y, double z) => new Vector3(x, y, z);
        static Camera JsCamera(Point3 position, Point3 lookat, Vector3 up, double flen) => new Camera(position, lookat, up, flen);
        static Light JsLight(Point3 position, Colour colour, double intensity) => new Light(position, colour, intensity);

        static void LoadContext(V8ScriptEngine engine)
        {

        }
    }
}
