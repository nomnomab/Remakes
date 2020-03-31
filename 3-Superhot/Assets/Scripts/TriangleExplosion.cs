using System.Collections;
using System.Collections.Generic;
using Attributes;
using DG.Tweening;
using UnityEngine;

public class TriangleExplosion : CustomBehaviour {
    [SerializeField] private Vector2 _forceRange = new Vector2(300, 500);
    [SerializeField] private float _size = 1;
    
    [RequireChild(0)] private SkinnedMeshRenderer _renderer;

    private List<Transform> _triangles;
    private Transform _boom;

    private void Start() {
        _triangles = new List<Transform>();
        SplitMesh();
    }

    public void Destroy(Vector3 impact) {
        Explode(impact);
    }

    public void SplitMesh() {
        _boom = new GameObject("Boom").transform;
        _boom.SetParent(transform);
        _boom.position = transform.position;
        Vector3 size = transform.localScale;

        Mesh mesh = _renderer.sharedMesh;
        Material[] materials = _renderer.materials;

        Vector3[] verts = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = mesh.uv;
        for (int submesh = 0; submesh < mesh.subMeshCount; submesh++) {
            int[] indices = mesh.GetTriangles(submesh);

            for (int i = 0; i < indices.Length; i += 12) {
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];
                
                for (int n = 0; n < 3; n++) {
                    int index = indices[i + n];
                    newVerts[n] = verts[index];
                    newUvs[n] = uvs[index];
                    newNormals[n] = normals[index];
                }

                Mesh triangleMesh = new Mesh {
                    vertices = newVerts, 
                    normals = newNormals, 
                    uv = newUvs, 
                    triangles = new[] { 0, 1, 2, 2, 1, 0 }
                };

                GameObject instance = new GameObject("Triangle " + i / 3);
                
                instance.transform.position = transform.position;
                instance.transform.rotation = transform.rotation;
                instance.transform.localScale = new Vector3(_size, _size, _size);
                
                instance.AddComponent<MeshRenderer>().material = materials[submesh];
                instance.AddComponent<MeshFilter>().mesh = triangleMesh;
                instance.AddComponent<BoxCollider>();
                instance.AddComponent<Rigidbody>();
                
                instance.SetActive(false);
                instance.transform.SetParent(_boom, true);
                
                _triangles.Add(instance.transform);
            }
        }
        
        _boom.Rotate(Vector3.forward, -90);
    }

    public void Explode(Vector3 impact) {
        _renderer.enabled = false;

        Vector3 explosionPos = impact;
        _boom.SetParent(null);

        foreach (Transform triangle in _triangles) {
            // yield return null;
            
            float force = Random.Range(_forceRange.x, _forceRange.y);
            
            triangle.gameObject.SetActive(true);
            triangle?.gameObject?.GetComponent<Rigidbody>().AddExplosionForce(force, explosionPos, 5);

            float time = Random.Range(0.9f, 2f);
            triangle.DOScale(Vector3.zero, time);
            Destroy(triangle.gameObject, time);
        }

        // yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}