using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject Player;
    public GameObject ExpCollector;
    public GameObject ballWeaponPrefab;

    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private CircleCollider2D CollectionRange;
    public List<BallManager> bolas = new List<BallManager>();

    [Header("Stats modificables")]
    [Range(1.5f, 10f)] public float playerSpeed = 3f;
    [Range(10f, 50f)] public float expCollectionRange = 10f;
    [Range(1, 4)] public int playerMaxHealth = 4;

    private void Start()
    {
        playerController = Player.GetComponent<PlayerController>();
        playerHealth = Player.GetComponent<PlayerHealth>();
        CollectionRange = ExpCollector.GetComponent<CircleCollider2D>();

        ActualizarListaDeBolas();
        AplicarStats();
    }

    public void AplicarMejora(PlayerUpgrade mejora)
    {
        switch (mejora.id)
        {
            case 1:
                AumentarVelocidad(0.5f);
                break;
            case 2:
                AumentarRangoExp(2f);
                break;
            case 3:
                AumentarTamañoABola(0.3f);
                break;
            case 4:
                AumentarLongitudABola(0.3f);
                break;
            case 5:
                ReducirPesoABola(0.3f);
                break;
            case 6:
                AumentarTamañoAMayor(0.2f);
                break;
            case 7:
                AumentarLongitudAMayor(0.2f);
                break;
            case 8:
                ReducirPesoAMenor(0.2f);
                break;
            case 9:
                AumentarTamañoAMenor(0.3f);
                break;
            case 10:
                AumentarLongitudAMenor(0.3f);
                break;
            case 11:
                ReducirPesoAMayor(0.3f);
                break;
            case 12:
                foreach (var bola in bolas)
                {
                    bola.Peso -= 0.2f;
                }
                break;
            case 13:
                foreach (var bola in bolas)
                {
                    bola.Longitud += 0.2f;
                }
                break;
            case 14:
                foreach (var bola in bolas)
                {
                    bola.Tamaño += 0.2f;
                }
                break;
            case 15:
                AgregarNuevaBola();
                break;
            case 16:
                AgregarNuevaBolaEncadenada();
                break;
            case 17:
                foreach (var bola in bolas)
                {
                    bola.Tamaño = bola.Tamaño/2;
                }
                for (int i = 0; i < 2; i++)
                {
                    AgregarNuevaBola();
                }
                break;
            case 18:
                foreach (var bola in bolas)
                {
                    bola.Longitud = bola.Longitud/2;
                }
                for (int i = 0; i < 2; i++)
                {
                    AgregarNuevaBola();
                }
                break;
            case 19:
                foreach (var bola in bolas)
                {
                    GameObject nuevaBola = Instantiate(ballWeaponPrefab, Player.transform.position, Quaternion.identity, transform);
                    BallManager manager = nuevaBola.GetComponent<BallManager>();
                    manager.StartPoint = Player;
                    manager.Tamaño = bola.Tamaño / 2;
                    bola.Tamaño = bola.Tamaño / 2;
                }
                break;
            case 20:
                AumentarVida(4);
                break;
            default:
                Debug.LogWarning("ID de mejora no reconocido: " + mejora.id);
                break;
        }

        AplicarStats();
        ActualizarListaDeBolas();
    }

    public void AplicarStats()
    {
        playerHealth.maxHealth = playerMaxHealth;
        playerController.moveSpeed = playerSpeed;
        CollectionRange.radius = expCollectionRange;
    }

    private void ActualizarListaDeBolas()
    {
        bolas.Clear();

        foreach (Transform child in transform)
        {
            BallManager ballManager = child.GetComponent<BallManager>();
            if (ballManager != null)
            {
                bolas.Add(ballManager);
            }
        }
    }

    public void AumentarVelocidad(float cantidad)
    {
        playerSpeed += cantidad;
    }

    public void AumentarRangoExp(float cantidad)
    {
        expCollectionRange += cantidad;
    }

    public void AumentarVida(int cantidad)
    {
        playerHealth.Heal(cantidad);
    }

    public void AgregarNuevaBola()
    {
        AgregarBola(Player);
    }

    public void AgregarNuevaBolaEncadenada()
    {
        BallManager bolaAleatoria = GetBolaAleatoria();
        AgregarBola(bolaAleatoria.Ball);
    }

    public void ReducirTamañoABola(float cantidad)
    {
        BallManager Bola = GetBolaAleatoria();
        Bola.Tamaño -= cantidad;
        Bola.ActualizarValores();
    }
    public void AumentarTamañoABola(float cantidad)
    {
        BallManager Bola = GetBolaAleatoria();
        Bola.Tamaño += cantidad;
        Bola.ActualizarValores();
    }
    public void ReducirLongitudABola(float cantidad)
    {
        BallManager Bola = GetBolaAleatoria();
        Bola.Longitud -= cantidad;
        Bola.ActualizarValores();
    }
    public void AumentarLongitudABola(float cantidad)
    {
        BallManager Bola = GetBolaAleatoria();
        Bola.Longitud += cantidad;
        Bola.ActualizarValores();
    }
    public void ReducirPesoABola(float cantidad)
    {
        BallManager Bola = GetBolaAleatoria();
        Bola.Peso -= cantidad;
        Bola.ActualizarValores();
    }
    public void AumentarPesoABola(float cantidad)
    {
        BallManager Bola = GetBolaAleatoria();
        Bola.Peso += cantidad;
        Bola.ActualizarValores();
    }

    public void AumentarTamañoAMenor(float cantidad)
    {
        BallManager Bola = GetMenorTamañoBolaValida();
        Bola.Tamaño += cantidad;
        Bola.ActualizarValores();
    }
    public void AumentarTamañoAMayor(float cantidad)
    {
        BallManager Bola = GetMayorTamañoBolaValida();
        Bola.Tamaño += cantidad;
        Bola.ActualizarValores();
    }
    public void AumentarLongitudAMenor(float cantidad)
    {
        BallManager Bola = GetMenorLongitudBolaValida();
        Bola.Longitud += cantidad;
        Bola.ActualizarValores();
    }
    public void AumentarLongitudAMayor(float cantidad)
    {
        BallManager Bola = GetMayorLongitudBolaValida();
        Bola.Longitud += cantidad;
        Bola.ActualizarValores();
    }
    public void AumentarPesoAMenor(float cantidad)
    {
        BallManager Bola = GetMenorPesoBolaValida();
        Bola.Peso += cantidad;
        Bola.ActualizarValores();
    }
    public void AumentarPesoAMayor(float cantidad)
    {
        BallManager Bola = GetMayorPesoBolaValida();
        Bola.Peso += cantidad;
        Bola.ActualizarValores();
    }

    public void ReducirTamañoAMenor(float cantidad)
    {
        BallManager Bola = GetMenorTamañoBolaValida();
        Bola.Tamaño -= cantidad;
        Bola.ActualizarValores();
    }
    public void ReducirTamañoAMayor(float cantidad)
    {
        BallManager Bola = GetMayorTamañoBolaValida();
        Bola.Tamaño -= cantidad;
        Bola.ActualizarValores();
    }
    public void ReducirLongitudAMenor(float cantidad)
    {
        BallManager Bola = GetMenorLongitudBolaValida();
        Bola.Longitud -= cantidad;
        Bola.ActualizarValores();
    }
    public void ReducirLongitudAMayor(float cantidad)
    {
        BallManager Bola = GetMayorLongitudBolaValida();
        Bola.Longitud -= cantidad;
        Bola.ActualizarValores();
    }
    public void ReducirPesoAMenor(float cantidad)
    {
        BallManager Bola = GetMenorPesoBolaValida();
        Bola.Peso -= cantidad;
        Bola.ActualizarValores();
    }
    public void ReducirPesoAMayor(float cantidad)
    {
        BallManager Bola = GetMayorPesoBolaValida();
        Bola.Peso -= cantidad;
        Bola.ActualizarValores();
    }

    //Logicas para crear y obtener bolas

    private void AgregarBola(GameObject conectadaA)
    {
        GameObject nuevaBola = Instantiate(ballWeaponPrefab, conectadaA.transform.position, Quaternion.identity, transform);
        BallManager manager = nuevaBola.GetComponent<BallManager>();
        manager.StartPoint = conectadaA;
    }

    private BallManager GetMayorTamañoBolaValida()
    {
        return GetMayorBolaValida(b => b.Tamaño, 3f);
    }
    private BallManager GetMenorTamañoBolaValida()
    {
        return GetMenorBolaValida(b => b.Tamaño, 0.5f);
    }
    private BallManager GetMayorLongitudBolaValida()
    {
        return GetMayorBolaValida(b => b.Longitud, 3f);
    }
    private BallManager GetMenorLongitudBolaValida()
    {
        return GetMenorBolaValida(b => b.Longitud, 0.8f);
    }
    private BallManager GetMayorPesoBolaValida()
    {
        return GetMayorBolaValida(b => b.Peso, 8f);
    }
    private BallManager GetMenorPesoBolaValida()
    {
        return GetMenorBolaValida(b => b.Peso, 1f);
    }
    private BallManager GetBolaAleatoria()
    {
        if (bolas.Count == 0) return null;

        int index = Random.Range(0, bolas.Count);
        return bolas[index];
    }

    private BallManager GetMayorBolaValida(System.Func<BallManager, float> selector, float maxValor)
    {
        BallManager mejor = null;
        float mejorValor = float.MinValue;

        foreach (var bola in bolas)
        {
            float valor = selector(bola);
            if (valor >= maxValor) continue;

            if (valor > mejorValor)
            {
                mejorValor = valor;
                mejor = bola;
            }
        }

        return mejor;
    }
    private BallManager GetMenorBolaValida(System.Func<BallManager, float> selector, float minValor)
    {
        BallManager mejor = null;
        float mejorValor = float.MaxValue;

        foreach (var bola in bolas)
        {
            float valor = selector(bola);
            if (valor <= minValor) continue;

            if (valor < mejorValor)
            {
                mejorValor = valor;
                mejor = bola;
            }
        }

        return mejor;
    }
}
