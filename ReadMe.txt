#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
# The Aurora C# Raytracing Library #
#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#

Aurora is a C# raytracer as a class library.
It comes with a test/driver program called Raytrace.

Aurora supports the usual primitive shapes including Quadrics and Composite Solid Geometry.

It handles shadows reflection and refraction.

Because its scene description language is C# it is very easy to construct complex scenes.

Vectors, Points and Transformations are all available and can be manipulated using operators for maximum clarity and conciseness.  Special care has been taken to ensure that operators return the appropriate types so that many geometric formulae can be copied straight from a text book. For example, the difference between two Points is a Vector.

The library is designed to be very easy to use so that driver code can remain focussed on the scene description and geometry without being cluttered with irrelevant detail.
  
Aurora was written for fun and because I wanted to see how a raytracer worked in detail.  All of the code* was written by me and there are undoubtedly things which can be improved both programmatically and physically. For example,the colour maths is just what I considered to be appropriate and there is a known issue with reflected ray energy.  The code is, as always, in a state of flux; having just had transparency, IOR, normal perturbation and basic textures added.  (These will also be the areas with new bugs!!)
There are no showstopper bugs and the physics is mostly believable however!  If there are errors then I assume that these are more likely to be in the material/scene/light code rather than in the geometry as the geometry code is much more likely to produce an obviously incorrect image.

[* Except for the Perlin noise function, ported from C code found on the web]

Wot's it good for!
~~~~~~~~~~~~~~~~~~
Aurora is a test-bed and a toy and is provided so that others may play or even add ray-tracing to their pet graphics project without having to start from scratch.  The code should be treated as pre-beta even though most of it works very well.

Legal Notice
~~~~~~~~~~~~
I retain copyright on the source but I ask for no more than an acknowledgement and the inclusion of this note if you use this code and distribute the result.  You may also distribute this archive as is.  If you have modified the source in anyway please make this clear if you redistribute.  You don't have to mark the changes, just make it clear that its a derivative.

I do not require that you distribute the source (as per the GPL) but I would ask that you help distribute the source where you can.

I can't image how anyone could damage anything/one with this source code but if you manage it then congratulations! Just remember that I will not be held liable in any way.  The risk of raytracing (!) is entirely yours...


~~~~~~~~~~~~~~~~~~~~~~~~
Aurora Version 1.0 notes
~~~~~~~~~~~~~~~~~~~~~~~~

Models
~~~~~~
Models are the visible objects that may be placed in the scene. Each is represented by a C# class.  The full hierarchy of model classes is given in the diagram below.

===========================================================================
Model                        All visible objects
|-Aggregate                  A collection of models transformed together
|-Triangle                   A single sided triangle
'-Solid *                    Solid models that can take part in CSG
  |-Sphere                   A Sphere
  |-Plane                    An infinite plane 
  |-Quadric                  A general quadric surface
  | |-Ellipsoid              A quadric ellipsoid
  | |-CylinderX              Cylinders in standard orientations
  | |-CylinderY  
  | |-CylinderZ
  | |-ConeX                  Double cones in standard orientations
  | |-ConeY
  | |-ConeZ
  | |-ParaboloidX            Paraboloids in ... you guessed it!
  | |-ParaboloidY
  | |-ParaboloidZ
  | |-Hyperboloid            A saddle surface
  | '-HyperboloidY           A power station cooling tower!
  |
  |-CSG *                    Base class of CSG objects
  | |-CSGUnion               Boolean OR
  | |-CSGIntersection        Boolean AND 
  | '-CSGDifference          Boolean AND NOT
  |
  '-CSGModel *               A wrapper for a CSG preset model
    |-Cuboid                 An axis-aligned cuboid
    '-Cube                   A cube, what more can I say?
===========================================================================

The classes marked * are abstract and are shown here for completeness.
This is a fairly complete set of basic raytracing models and is sufficient to build convincing models of inorganic subjects. You could, for example, make an excellent likeness of a computer with these models.

The CSG solids provide the real power because they allow solid models to be combined in simple ways to yield complex objects.  One object can be added to another (CSGUnion), Intersected with another (CSGIntersection) or cut from another (CSGDifference.)  When you consider that the objects being manipulated may themselves be CSG solids it is easy to see that complex shapes can be created with relative ease.

All models can be translated, rotated and scaled with the exception of the Sphere, which cannot be scaled.  Transformations can be applied to individual primitives or to a whole CSG object or Aggregate.  Hierarchically organised models can therefore have hierarcical transformations so that, for example, a robot arm can be simulated with its output axes being modelled directly by transformations.

Aggregates are provided to enable such transformation hierarchies.  An Aggregate looks like a CSGUnion and the models may overlap one another (only the nearest model will be seen.)  Aggregates are also very fast.  The limitations of Aggregates are that they are not solids (even when composed of solids) and cannot take part in CSG, and they are not a true union of their parts and may look odd when combined with transparency.

In general it is most convenient to create models as Solids and to use the solid operators (& | -) to do CSG as in the examples.  This allows you to ignore the CSG classes altogether and just use Solids, Triangles, Aggregates and the various preset shapes.  The final assembly can then be performed using one or more Aggregates.


Shader
~~~~~~
The shader supports diffuse and ambient illumination with Phong highlights.  Reflection, refraction and shadows are fully supported.  There is basic support for surface textures.  New texture functions can be added quite easily. Simple, point source lights of any colour may be placed in the scene. 

Use
~~~
Read the Raytrace.cs source file to see how to use Aurora.  It really is so straightforward in use that the examples say it all.  It is obviously a very good idea to check out the library source but the examples have more than enough info to get you started.  The Raytrace.cs file is a bit messy but the examples are simple and readable.  The sample scenes are not meant to show off the raytracer, they are just the test scenes that I am presently using.  Start out by defining a scene for button 8.

BTW
~~~
The source uses double[][] matrices rather than double[,] because the former is reported to be faster.  This is supported by my own experiments. 

Finally...
~~~~~~~~~~
The Aurora.Colour class is spelled correctly so it may catch out any unwary Americans (just as I am caught out by the System.Color class!) I left the colour names alone (gray/grey) as these are standard names and are therefore properly mispelled ;)

