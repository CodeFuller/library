using System;
using CF.Library.Patterns;
using NUnit.Framework;

namespace CF.Library.UnitTests.CF.Library.Patterns
{
	class FakeEventArgs : MultilayerEventArgs
	{
	}

	class FakeEventArgs2 : MultilayerEventArgs
	{
	}

	class Layer
	{
		public bool EventFired { get; set; }

		public void OnEvent(object sender, FakeEventArgs e)
		{
			EventFired = true;
		}

		public void OnEventMarkHandled(object sender, FakeEventArgs e)
		{
			EventFired = true;
			e.Handled = true;
		}
	}

	class Layer1 : Layer
	{
	}

	class Layer2 : Layer
	{
	}

	[TestFixture]
	public class MultilayerEventManagerTests
	{
		[TearDown]
		public void Dispose()
		{
			MultilayerEventManager.Clear();
		}

		[Test]
		public void RegisterLowerLayer_ForLowerInstance_AddsParentRelationship()
		{
			var layer1 = new Layer1();
			var layer2 = new Layer2();

			MultilayerEventManager.RegisterLowerLayer<FakeEventArgs>(layer1, layer2);

			var registeredParent = MultilayerEventManager.Parents[typeof (FakeEventArgs)][layer2].Target;
			Assert.IsTrue(ReferenceEquals(registeredParent, layer1));
		}

		[Test]
		public void RegisterLowerLayerForEvents_WhenCalled_AddsParentRelationship()
		{
			var layer1 = new Layer1();
			var layer2 = new Layer2();

			MultilayerEventManager.RegisterLowerLayerForEvents(layer1, layer2, typeof(FakeEventArgs), typeof(FakeEventArgs2));

			var registeredParent1 = MultilayerEventManager.Parents[typeof(FakeEventArgs)][layer2].Target;
			var registeredParent2 = MultilayerEventManager.Parents[typeof(FakeEventArgs2)][layer2].Target;

			Assert.IsTrue(ReferenceEquals(registeredParent1, layer1));
			Assert.IsTrue(ReferenceEquals(registeredParent2, layer1));
		}

		[Test]
		public void RegisterHandler_ForSingleLayer_AddsHandler()
		{
			var layer1 = new Layer1();

			MultilayerEventManager.RegisterHandler<Layer1, FakeEventArgs>(layer1, (t, s, e) => t.OnEvent(s, e));

			Assert.IsTrue(MultilayerEventManager.Handlers[typeof(FakeEventArgs)].Count == 1);
		}

		[Test]
		public void UnregisterHandler_ForSingleLayer_RemovesHandler()
		{
			var layer1 = new Layer1();
			MultilayerEventManager.RegisterHandler<Layer1, FakeEventArgs>(layer1, (t, s, e) => t.OnEvent(s, e));

			MultilayerEventManager.UnregisterHandler<FakeEventArgs>(layer1);

			Assert.IsTrue(MultilayerEventManager.Handlers.Count == 0);
		}

		[Test]
		public void UnregisterInstance_ForLowerLayer_RemovesParentRelationship()
		{
			var layer1 = new Layer1();
			var layer2 = new Layer2();
			MultilayerEventManager.RegisterLowerLayer<FakeEventArgs>(layer1, layer2);

			MultilayerEventManager.UnregisterInstance(layer2);

			Assert.IsTrue(MultilayerEventManager.Parents.Count == 0);
		}

		[Test]
		public void UnregisterInstance_ForUpperLayer_RemovesParentRelationship()
		{
			var layer1 = new Layer1();
			var layer2 = new Layer2();
			MultilayerEventManager.RegisterLowerLayer<FakeEventArgs>(layer1, layer2);

			MultilayerEventManager.UnregisterInstance(layer1);

			Assert.IsTrue(MultilayerEventManager.Parents.Count == 0);
		}

		[Test]
		public void UnregisterInstance_WhenCalled_RemovesHandler()
		{
			var layer1 = new Layer1();
			MultilayerEventManager.RegisterHandler<Layer1, FakeEventArgs>(layer1, (t, s, e) => t.OnEvent(s, e));

			MultilayerEventManager.UnregisterInstance(layer1);

			Assert.IsTrue(MultilayerEventManager.Handlers.Count == 0);
		}

		[Test]
		public void TriggerEvent_ForSingleLayerWithoutHandler_WorksCorrectly()
		{
			var layer1 = new Layer1();

			MultilayerEventManager.TriggerEvent(layer1, new FakeEventArgs());

			Assert.IsFalse(layer1.EventFired);
		}

		[Test]
		public void TriggerEvent_ForSingleLayerWithHandler_CallsHandler()
		{
			var layer1 = new Layer1();
			MultilayerEventManager.RegisterHandler<Layer1, FakeEventArgs>(layer1, (t, s, e) => t.OnEvent(s, e));

			MultilayerEventManager.TriggerEvent(layer1, new FakeEventArgs());

			Assert.IsTrue(layer1.EventFired);
		}

		[Test]
		public void TriggerEvent_ForSecondLayerOnTwoLayers_CallsCorrectHandler()
		{
			var layer1 = new Layer1();
			var layer2 = new Layer2();
			MultilayerEventManager.RegisterLowerLayer<FakeEventArgs>(layer1, layer2);
			MultilayerEventManager.RegisterHandler<Layer2, FakeEventArgs>(layer2, (t, s, e) => t.OnEvent(s, e));

			MultilayerEventManager.TriggerEvent(layer2, new FakeEventArgs());

			Assert.IsFalse(layer1.EventFired);
			Assert.IsTrue(layer2.EventFired);
		}

		[Test]
		public void TriggerEvent_ForFirstLayerOnTwoLayers_CallsCorrectHandler()
		{
			var layer1 = new Layer1();
			var layer2 = new Layer2();
			MultilayerEventManager.RegisterLowerLayer<FakeEventArgs>(layer1, layer2);
			MultilayerEventManager.RegisterHandler<Layer1, FakeEventArgs>(layer1, (t, s, e) => t.OnEvent(s, e));
			MultilayerEventManager.RegisterHandler<Layer2, FakeEventArgs>(layer2, (t, s, e) => t.OnEvent(s, e));

			MultilayerEventManager.TriggerEvent(layer1, new FakeEventArgs());

			Assert.IsTrue(layer1.EventFired);
			Assert.IsFalse(layer2.EventFired);
		}

		[Test]
		public void TriggerEvent_ForAllLayersOnTwoLayers_CallsCorrectHandlers()
		{
			var layer1 = new Layer1();
			var layer2 = new Layer2();
			MultilayerEventManager.RegisterLowerLayer<FakeEventArgs>(layer1, layer2);
			MultilayerEventManager.RegisterHandler<Layer1, FakeEventArgs>(layer1, (t, s, e) => t.OnEvent(s, e));
			MultilayerEventManager.RegisterHandler<Layer2, FakeEventArgs>(layer2, (t, s, e) => t.OnEvent(s, e));

			MultilayerEventManager.TriggerEvent(layer2, new FakeEventArgs());

			Assert.IsTrue(layer1.EventFired);
			Assert.IsTrue(layer2.EventFired);
		}

		[Test]
		public void TriggerEvent_WithLowerTypeInChain_WorksCorrectly()
		{
			var layer1 = new Layer1();
			var layer2 = new Layer2();
			MultilayerEventManager.RegisterLowerLayer<FakeEventArgs>(layer1, layer2.GetType());
			MultilayerEventManager.RegisterHandler<Layer1, FakeEventArgs>(layer1, (t, s, e) => t.OnEvent(s, e));
			MultilayerEventManager.RegisterHandler<Layer2, FakeEventArgs>(layer2, (t, s, e) => t.OnEvent(s, e));

			MultilayerEventManager.TriggerEvent(layer2, new FakeEventArgs());

			Assert.IsTrue(layer1.EventFired);
			Assert.IsTrue(layer2.EventFired);
		}

		[Test]
		public void TriggerEvent_WhenLayerMarkedEventAsHandled_StopsCallingHandlers()
		{
			var layer1 = new Layer1();
			var layer2 = new Layer2();
			MultilayerEventManager.RegisterLowerLayer<FakeEventArgs>(layer1, layer2);
			MultilayerEventManager.RegisterHandler<Layer1, FakeEventArgs>(layer1, (t, s, e) => t.OnEvent(s, e));
			MultilayerEventManager.RegisterHandler<Layer2, FakeEventArgs>(layer2, (t, s, e) => t.OnEventMarkHandled(s, e));

			MultilayerEventManager.TriggerEvent(layer2, new FakeEventArgs());

			Assert.IsFalse(layer1.EventFired);
			Assert.IsTrue(layer2.EventFired);
		}

		[Test]
		public void Clear_WithAddedHandlersAndLayers_RemovesAllData()
		{
			var layer1 = new Layer1();
			var layer2 = new Layer2();
			MultilayerEventManager.RegisterLowerLayer<FakeEventArgs>(layer1, layer2);
			MultilayerEventManager.RegisterHandler<Layer1, FakeEventArgs>(layer1, (t, s, e) => t.OnEvent(s, e));

			MultilayerEventManager.Clear();

			Assert.IsTrue(MultilayerEventManager.Parents.Count == 0);
			Assert.IsTrue(MultilayerEventManager.Handlers.Count == 0);
		}

		[Test]
		public void RegisteredHandlers_AfterRemoveDeadReferences_StillExist()
		{
			var layer1 = new Layer1();
			MultilayerEventManager.RegisterHandler<Layer1, FakeEventArgs>(layer1, (t, s, e) => t.OnEvent(s, e));

			ForceGC();
			MultilayerEventManager.RemoveDeadReferences();
			MultilayerEventManager.TriggerEvent(layer1, new FakeEventArgs());

			Assert.IsTrue(MultilayerEventManager.Handlers.Count == 1);
			Assert.IsTrue(layer1.EventFired);
		}

		[Test, TestCaseSource(nameof(MethodsWhichCall_RemoveDeadReferences))]
		public void ModificationMethods_WhenCalledEnoughTimes_RemoveDeadReferences(Action testedMethodCall)
		{
			if (testedMethodCall == null)
			{
				throw new ArgumentNullException(nameof(testedMethodCall));
			}

			WeakReference layer1Ref;
			WeakReference layer2Ref;
			FillDeadReferencesTestData(out layer1Ref, out layer2Ref);
			ForceGC();

			for (var i = 0; i < MultilayerEventManager.CallsBetweenDeadReferencesRemoving; ++i)
			{
				testedMethodCall();
			}

			Assert.IsFalse(MultilayerEventManager.Parents.ContainsKey(typeof(FakeEventArgs)));
			Assert.IsFalse(MultilayerEventManager.Handlers.ContainsKey(typeof(FakeEventArgs)));
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "This field is used by NUnit framework as test data source")]
		private static readonly Action[] MethodsWhichCall_RemoveDeadReferences =
		{
			() => MultilayerEventManager.RegisterLowerLayer<FakeEventArgs2>(new Layer1(), new Layer2()),
			() => MultilayerEventManager.RegisterLowerLayer<FakeEventArgs2>(new Layer1(), typeof(Layer2)),
			() => MultilayerEventManager.RegisterHandler<Layer1, FakeEventArgs2>(new Layer1(), (t, s, e) => {}),
			() => MultilayerEventManager.UnregisterHandler<FakeEventArgs2>(new Layer1()),
			() => MultilayerEventManager.UnregisterInstance(new Layer1()),
			() => MultilayerEventManager.TriggerEvent(new Layer1(), new FakeEventArgs2()),
		};

		/// <remarks>
		/// This method is required because GC in debug version will not collect variables that are on stack
		/// See for details: http://stackoverflow.com/questions/37462378/why-c-sharp-garbage-collection-behavior-differs-for-release-and-debug-executable
		/// </remarks>
		private void FillDeadReferencesTestData(out WeakReference layer1Ref, out WeakReference layer2Ref)
		{
			var layer1 = new Layer1();
			layer1Ref = new WeakReference(layer1);

			var layer2 = new Layer2();
			layer2Ref = new WeakReference(layer2);

			MultilayerEventManager.RegisterLowerLayer<FakeEventArgs>(layer1, layer2);
			MultilayerEventManager.RegisterHandler<Layer1, FakeEventArgs>(layer1, (t, s, e) => t.OnEvent(s, e));
			MultilayerEventManager.RegisterHandler<Layer2, FakeEventArgs>(layer2, (t, s, e) => t.OnEvent(s, e));
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", Justification = "Call to GC.Collect() is required to test releasing of weakly referenced objects")]
		private void ForceGC()
		{
			GC.Collect();
			GC.WaitForFullGCComplete();
		}
	}
}
