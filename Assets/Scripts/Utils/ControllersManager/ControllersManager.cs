using System;
using System.Collections.Generic;
using Game.UI.Controllers;
using Utils.DictionaryExtension;

namespace Utils.ControllersManager
{
    public class ControllersManager
    {
        private readonly Dictionary<Type,Controller> _controllers = new ();

        public ControllersManager(params Controller[] initControllers)
        {
            foreach (var controller in initControllers)
                _controllers.Add(controller.GetType(), controller);
        }

        public void Add<T>() where T : Controller=> _controllers.Add(typeof(T), Activator.CreateInstance<T>());
        public void Remove<T>() where T : Controller => _controllers.Remove(typeof(T));
        public void Enable<T>() where T : Controller => _controllers[typeof(T)].Enable();
        public void Disable<T>() where T : Controller => _controllers[typeof(T)].Disable();
        public T Get<T>() where  T : Controller => (T)_controllers[typeof(T)]; 

        public void Initialize() => _controllers.ForEachValue( item => item.Enable());
        public void Terminate() => _controllers.ForEachValue( item => item.Disable());
    }
}