using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireControl : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawn;   
    
    void Update()
    {
        if (!isLocalPlayer) return;       
        ShootBullet();
    }
    void ShootBullet()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CmdShoot();            
        }
    }
    void CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);        
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 18;      
        Destroy(bullet, 2.5f);
    }
    [Command]
    void CmdShoot()
    {
        CreateBullet();
        RpcCreateBullet();
    }
    [ClientRpc]
    void RpcCreateBullet()
    {
        if (!isServer)
        {
            CreateBullet();
        }        
    }
}
