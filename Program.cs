using MagicCARpet.Components;
using MagicCARpet.Helper;
using MagicCARpet.Systems;
using MvvmGen.Events;
using StereoKit;

namespace MagicCARpet;

class Program
{
	public static IEventAggregator EventAggregator { get; set; }

	static void Main(string[] args)
	{
        SK.AddStepper(new PassthroughFBExt { Enabled = true });

        // Initialize StereoKit
        SKSettings settings = new SKSettings
		{
			appName = "MagicCARpet",
			assetsFolder = "Assets",
		};
		if (!SK.Initialize(settings))
			return;

		EventAggregator = new EventAggregator();

		//Activate steppers
		SK.AddStepper(new GestureRecognizerComponent(EventAggregator));
		SK.AddStepper(new StateSystem(EventAggregator));
		SK.AddStepper(new RoadManagerComponent(EventAggregator));

        // Create assets used by the app
        Pose  cubePose = new Pose(0, 0, -0.5f);
		Model cube     = Model.FromMesh(
			Mesh.GenerateRoundedCube(Vec3.One*0.1f, 0.02f),
			Material.UI);

		Matrix   floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
		Material floorMaterial  = new Material("floor.hlsl");
		floorMaterial.Transparency = Transparency.Blend;


		// Core application loop
		SK.Run(() => {
			if (SK.System.displayType == Display.Opaque)
				Mesh.Cube.Draw(floorMaterial, floorTransform);

			UI.Handle("Cube", ref cubePose, cube.Bounds);
			cube.Draw(cubePose.ToMatrix());
		});
	}
}