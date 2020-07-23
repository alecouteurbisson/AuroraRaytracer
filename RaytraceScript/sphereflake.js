function Build(scene)
{
    var camera = new Camera(new Point3(5.0, 8.0, -10.0),    // Position
                            new Point3(0.0, 0.0, 0.0),      // Look at
                            Vector3.UnitY,                  // Up
                            6.0,                            // Focal length
                            scene.Size);                    // Aspect
    scene.Camera = camera;

    var l = new Light(new Point3(-1.0, 5.0, -10.0),  // Position
            new Colour(1.0),                         // Colour
            0.5);                                    // Intensity
    scene.Add(l);

    l = new Light(new Point3(10.0, -5.0, 0.0),   // Position
            new Colour(1.0),                     // Colour
            0.5);                                // Intensity
    scene.Add(l);

    scene.Antialias = true;

    fin = new Finish(0.5, 0.5, 0.5, 0.5);
    pig = new Pigment(Colour.Tomato);

    // pig = new Pigment(Colour.Lime, Colour.Navy, RealFunctions.Perlin);
    pig.Scale = 0.1;
    mat = new Material(pig, fin);

    //Material flakematerial = new Material(new Colour(0.8, 0.5, 0.2), 0.4, 0.2, 30.0, 0.4);
    var a = new Aggregate(mat);
    sphereflake(a, Point3.Origin, 1.0, 0, 0.5, 4);
    // Create a top level bounding volume
    var bound = new Sphere(Point3.Origin, 2.9);
    a.Bound = bound;
    scene.Add(a);
}

function sphereflake(a, p,  r, orient, ratio, level)
{
    a.Add(new Sphere(p, r));
    if(level > 0)
    {
        var dist = (1.0 + ratio) * r;
        var rr = r * ratio;
        var X = new Vector3(dist, 0.0, 0.0);
        var Y = new Vector3(0.0, dist, 0.0);
        var Z = new Vector3(0.0, 0.0, dist);

        if(orient != 1) sphereflake(a, p.Add(X), rr, 2, ratio, level - 1);
        if(orient != 2) sphereflake(a, p.Sub(X), rr, 1, ratio, level - 1);
        if(orient != 3) sphereflake(a, p.Add(Y), rr, 4, ratio, level - 1);
        if(orient != 4) sphereflake(a, p.Sub(Y), rr, 3, ratio, level - 1);
        if(orient != 5) sphereflake(a, p.Add(Z), rr, 6, ratio, level - 1);
        if(orient != 6) sphereflake(a, p.Sub(Z), rr, 5, ratio, level - 1);
    }
}