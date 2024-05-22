using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagerBase<T> : Singleton<ManagerBase<T>> where T : HasID {
  public Transform prefab;

  private Dictionary<int, Transform> transforms = new Dictionary<int, Transform>();


  public T Get(int id) {
    return transforms[id].GetComponent<T>();
  }
  public List<T> GetAll() {
    return transforms.Values.Select(x => x.GetComponent<T>()).ToList();
  }

  public virtual void Add(int id) {
    var newTransform = Instantiate(prefab);

    var script = newTransform.GetComponent<T>();
    script.ID = id;

    transforms[id] = newTransform;
  }
  public virtual void Remove(int id) {
    Destroy(transforms[id].gameObject);
    transforms.Remove(id);
  }
}
