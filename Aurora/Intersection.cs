using System.Collections.Generic;

namespace Aurora
{
  #region Intersection
  // A record of a ray surface intersection
  public class Intersection
  {
    internal Point3    location;    // Hit location
    internal double    distance;    // Ray parameter at location
    internal bool      entering;    // True if ray entering model
    internal Model     model;       // The primitive model hit
    internal Material  medium;      // Material preceding intersection 

    public Intersection()
    {
      distance = double.PositiveInfinity;
    }

    public Intersection(Point3 location, double distance,
                        bool entering, Model model, Material medium)
    {
      this.location = location;
      this.distance = distance;
      this.entering = entering;
      this.model = model;
      if(medium == null)
        this.medium = Material.NullMaterial;
      else
        this.medium = medium;
    }

    public void Flip()
    {
      entering = !entering;
    }

    // Compare intersections by distance (ray parameter)
    public static bool operator<(Intersection i, Intersection j)
    {
      return i.distance < j.distance;
    }

    public static bool operator>(Intersection i, Intersection j)
    {
      return i.distance > j.distance;
    }

    public Point3 Location
    {
      get { return location; }
    }

    public double Distance
    {
      get { return distance; }
    }

    public bool Entering
    {
      get { return entering; }
    }

    public Model Model
    {
      set { model = value; }
      get { return model; }
    }

    public Material Medium
    {
      get { return medium; }
    }
  }
  #endregion

  #region IntersectionList
  // A sorted list of intersections of a ray with the scene
  // The list is not sorted directly but is compiled in a sorted order
  public class IntersectionList : Queue<Intersection>
  {
    public void Drop()
    {
      Dequeue();
    }

    public bool Empty()
    {
      return Count == 0;
    }

    public bool Entering()
    {
      return Peek().Entering;
    }

    public double Distance()
    {
      return (Count == 0) ?
        double.PositiveInfinity :
        Peek().Distance;
    }
  }
  #endregion
}

