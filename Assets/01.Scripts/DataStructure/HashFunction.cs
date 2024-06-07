using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class HashFunction : MonoBehaviour
{
    HashAlgorithm _hash;

    void Start()
    {
        _hash = SHA512.Create();
        HashIt();
    }

    public void HashIt()
    {

        //byte[] buffer = BitConverter.GetBytes("2432323");
        byte[] buffer = Encoding.Unicode.GetBytes("Hello world!!!");
        byte[] hashedBuffer = _hash.ComputeHash(buffer);

        //Debug.Log(Encoding.Default.GetString(hashedBuffer));
    }
}
