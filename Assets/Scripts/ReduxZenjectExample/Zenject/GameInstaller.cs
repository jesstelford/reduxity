﻿using System;
using UnityEngine;
using Zenject;

namespace Reduxity.Example.Zenject {
    /// <summary>
    /// See here for more details:
    /// https://github.com/modesttree/zenject#installers
    /// </summary>
	public class GameInstaller : MonoInstaller<GameInstaller> {

        [Inject]
        Settings settings_ = null;

        /// <summary>
        /// Install bindings into the direct injection container.
        /// Note: order matters because of initializers!
        /// </summary>
		public override void InstallBindings() {
            // 1. init default nested state object
			InstallState();
            // 2. reducers are injected into App
            InstallReducers();
            // 3. create store from reducers
            InstallApp();
            // 4. init store state subscribers
            InstallComponents();
            // 5. install renderers
            InstallContainers();
		}

        /// <summary>
        /// Initialize each state as a single instance. Bind Initialize() to each.
        /// </summary>
        private void InstallState() {
            Container.Bind<IInitializable>().To<CharacterState>().AsSingle();
            Container.Bind<CharacterState>().AsSingle();
            Container.Bind<IInitializable>().To<CameraState>().AsSingle();
            Container.Bind<CameraState>().AsSingle();
			Container.Bind<State>().AsSingle(); // TODO: should this be transient?
		}

        /// <summary>
        /// Install reducers and use `WhenInjectedIno` in order to ensure that they are
        /// only available when used by App to create the Store. This is because nothing
        /// but the Store intiailizer should interact directly with reducers; only actions
        /// that are dispatched will.
        /// </summary>
        private void InstallReducers() {
            Container.Bind<Movement.Reducer>().AsSingle().WhenInjectedInto<App>();
            Container.Bind<Look.Reducer>().AsSingle().WhenInjectedInto<App>();
        }

        /// <summary>
        /// Install the App, which contains the Store that is initialized.
        /// </summary>
        private void InstallApp() {
            Container.Bind<IInitializable>().To<App>().AsSingle();
            Container.Bind<App>().AsSingle();
        }

        /// <summary>
        /// Install Components that subscribe to and render state changes.
        /// </summary>
        private void InstallComponents() {
            Container.Bind<IInitializable>().To<MoveCharacter>().AsSingle();
            Container.Bind<MoveCharacter>().AsSingle();
            Container.Bind<IInitializable>().To<MoveCamera>().AsSingle();
            Container.Bind<MoveCamera>().AsSingle();
        }

        /// <summary>
        /// Install Containers, which are MonoBehaviours that may include mah components.
        /// </summary>
        private void InstallContainers() {
        }

		[Serializable]
		public class Settings {
		}
	}
}
