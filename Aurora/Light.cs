namespace Aurora
{
  #region Light
  // A simple anisotropic light source with shading object caching to
  // optimise shadow calculation

  public class Light
  {
    public Colour Colour { get; }
    public double Intensity { get; }
    public Point3 Location { get; }

    public Light(Point3 location, Colour colour, double intensity)
    {
      Location = location;
      Colour = colour;
      Intensity = intensity;
    }

    // Default to white light at origin
    public Light()
      : this(new Point3(0.0, 0.0, 0.0), new Colour(1.0, 1.0, 1.0), 1.0)
    {}

    // Determine the colour contribution of this light falling
    // unshadowed on the intersection 
    public Colour Illumination(Intersection hit, Vector3 normal)
    {
      var lray = new Ray(Location, hit.Location);        // Incident light ray
      return hit.Model.Material.Illumination(hit.Location, normal, lray, this);
    }
  }
  #endregion
}

