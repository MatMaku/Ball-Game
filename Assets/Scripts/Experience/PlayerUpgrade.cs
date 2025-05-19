using UnityEngine;

public enum Rareza
{
    Comun,
    Rara,
    Epica
}

[CreateAssetMenu(fileName = "NuevaMejora", menuName = "Mejoras/PlayerUpgrade")]
public class PlayerUpgrade : ScriptableObject
{
    public int id;                
    public string nombre;          
    [TextArea] public string descripcion;  
    public Rareza rareza;           
}
