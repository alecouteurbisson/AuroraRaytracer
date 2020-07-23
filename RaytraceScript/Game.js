// A Game
function Build(scene)
{
    var camera = new Camera(new Point3(4.0, 12.0, -15.0), // Location
                            new Point3(0.0, 0.0, 0.0),    // Look at
                            new Vector3(0.0, 1.0, 0.0),   // Sky Vector
                            4.0,                          // Focal length
                            scene.Size);                  // Aspect ratio

    scene.Camera = camera;

    l = new Light(new Point3(-1.0, 5.0, -10.0),  // Position
                  new Colour(1.1),               // Colour
                  1.0);                          // Intensity
    scene.Add(l);
    scene.Antialias = true;
    scene.Gamma = 0.8;



    matt = new Finish(1.0, 0.0, 0.0, 0.0);
    boardmaterial = new Material(new Colour(0.204, 0.467, 0.620), matt);
    c = new Cuboid(new Point3(0.0, 0.5, 0.0), 4.0, 1.0, 4.0, boardmaterial);

    metal = new Finish(0.3, 0.4, 50.0, 0.5);
    gold = new Material(new Colour(0.851, 0.812, 0.067), metal);
    silver = new Material(new Colour(0.820, 0.969, 0.988), metal);

    // Form board and pieces using CSG
    s = new Sphere(new Point3(-1.4, 1.1, 1.4), 0.45, gold);
    c = glue(c, s);
    s = new Sphere(new Point3(0.0, 1.1, 1.4), 0.45, boardmaterial);
    c = cut(c, s);
    s = new Sphere(new Point3(1.4, 1.1, 1.4), 0.45, gold);
    c = glue(c, s);

    s = new Sphere(new Point3(-1.4, 1.1, 0.0), 0.45, silver);
    c = glue(c, s);
    s = new Sphere(new Point3(0.0, 1.1, 0.0), 0.45, gold);
    c = glue(c, s);
    s = new Sphere(new Point3(1.4, 1.1, 0.0), 0.45, silver);
    c = glue(c, s);

    s = new Sphere(new Point3(-1.4, 1.1, -1.4), 0.45, gold);
    c = glue(c, s);
    s = new Sphere(new Point3(0.0, 1.1, -1.4), 0.45, boardmaterial);
    c = cut(c, s);
    s = new Sphere(new Point3(1.4, 1.1, -1.4), 0.45, silver);
    c = glue(c, s);

    scene.Add(c);

    // Unplayed pieces
    s = new Sphere(new Point3(-3.1, 0.45, -1.0), 0.45, gold);
    scene.Add(s);
    s = new Sphere(new Point3(3.0, 0.45, 1.4), 0.45, silver);
    scene.Add(s);
    s = new Sphere(new Point3(3.3, 0.45, -1.2), 0.45, silver);
    scene.Add(s);

    // The floor
    floormaterial = new Material(new Colour(0.2, 1.0, 0.4), matt);
    scene.Add(new Plane(new Point3(0.0, 0.0, 0.0),    // Point in plane
                        new Vector3(0.0, 1.0, 0.0),   // Normal
                        floormaterial));              // Material
}

