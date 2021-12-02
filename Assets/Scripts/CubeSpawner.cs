using System.Collections.Generic;
using Script;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using System.IO; 

public class CubeSpawner : MonoBehaviour
{
    public GameObject CubePrefab;
    public List<GameObject> _cubeList = new List<GameObject>();
    private string _savedData;

    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            GameObject instantiate = Instantiate(CubePrefab, transform.position, Quaternion.identity );
            _cubeList.Add(instantiate);
            Color col = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            instantiate.GetComponent<MeshRenderer>().material.color = col;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            // Serialization process
            FileSave fileSave = new FileSave(_cubeList);
            _savedData = JsonUtility.ToJson(fileSave);
            File.WriteAllText(Application.streamingAssetsPath + "/data.json", _savedData);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            // Clear state
            foreach (GameObject o in _cubeList)
            {
                Destroy(o);
            }

            _cubeList.Clear();
            // Deserialization process
            _savedData = File.ReadAllText(Application.streamingAssetsPath + "/data.json");
            FileSave fileSave = JsonUtility.FromJson<FileSave>(_savedData);
            
            foreach (SerializableTransform serializableTransform in fileSave.SerializableTransforms)
            {
                GameObject instantiate = Instantiate(CubePrefab, transform.position, Quaternion.identity);
                instantiate.transform.position = serializableTransform.Position;
                instantiate.transform.rotation = serializableTransform.Rotation;
                instantiate.transform.localScale = serializableTransform.Scale;
                instantiate.GetComponent<Rigidbody>().velocity = serializableTransform.Velocity;
                instantiate.GetComponent<MeshRenderer>().material.color = serializableTransform.Colour;
                _cubeList.Add(instantiate);
            }
        }
    }
}