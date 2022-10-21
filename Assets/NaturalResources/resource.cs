using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resource : MonoBehaviour
{
    float health;
    [SerializeField] int maxHealth=100;
    [SerializeField] bool Collectable=false;
    
    Mesh originalMesh;
    Material originalMaterial;
    [SerializeField] Mesh[] meshes;
    [SerializeField] Mesh lastMesh;
    [SerializeField] Material lastMaterial;

    [SerializeField] float lastMeshTime=10;

    [SerializeField] Item LootItem;
    [SerializeField] int LootAmount = 5;
 
    Animator animator;
    GameObject Object;
    // Start is called before the first frame update
    void Start()
    {
        Object = this.gameObject;
        maxHealth = (int)(maxHealth * transform.lossyScale.x);
        LootAmount = (int)(LootAmount * transform.lossyScale.x);
        
        health = maxHealth;
        animator = GetComponentInChildren<Animator>();
        originalMesh = GetComponentInChildren<MeshFilter>().mesh;
        originalMaterial = GetComponentInChildren<MeshRenderer>().material;
    }
    public bool Damage(float amount)
    {
        health -= amount;
        if (health > 0) 
        {
            float diff = maxHealth/(meshes.Length+1);
            for(int i=1;i<meshes.Length+1;i++)
            {
                if (health <= diff * i)
                {
                    GetComponentInChildren<MeshFilter>().mesh=meshes[i-1]; break;
                }
            }
            return true;
        } 
        if (!Collectable)
            this.gameObject.layer *= 0;
        if (animator != null) animator.SetTrigger("Destroyed");
        else ShowLastMesh();

        return false;
    }
    void ShowLastMesh()
    {
        GetComponentInChildren<MeshFilter>().mesh = lastMesh;
        if(lastMaterial!=null) GetComponentInChildren<MeshRenderer>().material = lastMaterial;
        StartCoroutine(ShowWait());  
    }
    IEnumerator ShowWait()
    {
        yield return new WaitForSeconds(lastMeshTime);

        Inventory.instance.Add(LootItem,LootAmount);
        if (Collectable) ReturnStart();
        else DestroyObject();
    }
    void ReturnStart()
    {
        health = maxHealth;
        GetComponentInChildren<MeshFilter>().mesh = originalMesh;
        GetComponentInChildren<MeshRenderer>().material = originalMaterial;
    }
    private void DestroyObject()
    {
        Destroy(Object);
    }
}
