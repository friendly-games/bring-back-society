using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BringBackSociety.Services
{
  /// <summary> Contains all services for the system. </summary>
  public static class ServiceManager
  {
    /// <summary> All services that have been registered. </summary>
    private static readonly Dictionary<Type, object> RegisteredServices;

    /// <summary> Static constructor. </summary>
    static ServiceManager()
    {
      RegisteredServices = new Dictionary<Type, object>();
    }

    /// <summary> Gets instance of the given service. </summary>
    /// <exception cref="KeyNotFoundException"> Thrown when a Key Not Found error condition occurs. </exception>
    /// <typeparam name="T"> The service type to register </typeparam>
    /// <returns> The service. </returns>
    public static T GetService<T>()
    {
      lock (RegisteredServices)
      {
        if (!RegisteredServices.ContainsKey(typeof (T)))
          throw new KeyNotFoundException("Service has not been registered.");

        return (T) RegisteredServices[typeof (T)];
      }
    }

    /// <summary> Provides an implementation of a given service. </summary>
    /// <typeparam name="T"> The service interface to provide an implementation for. </typeparam>
    /// <param name="instance"> The instance of the given interface. </param>
    /// <returns> An instance of an object that implements the T service. </returns>
    public static void Provide<T>(T instance)
    {
      lock (RegisteredServices)
      {
        if (!RegisteredServices.ContainsKey(typeof (T)))
          throw new DuplicateNameException("Service has already been registered.");
        if (!typeof (T).IsInterface)
          throw new Exception("T is not an interface");

        RegisteredServices[typeof (T)] = instance;
      }
    }
  }
}