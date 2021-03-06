﻿
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RatMovementScript : Enemy
{
    public Animator anim; //Define el animator para la rata
    public Rigidbody2D colision;// Le da colisiones
    private int cycles;
    public int Velocidad;
    public Vector2 random;
    private bool derecha = true;

    void Start()
    {
        colision = GetComponent<Rigidbody2D>();//Inicializa las colisiones
        random = Random.insideUnitCircle;
        random = random.normalized;
        Run(random);
        cycles = 0;
    }
    private void Run(Vector2 R)// Le da velocidad
    {
        colision.velocity = R * MoveSpeed;
    }
    private void Flip()// Voltea la rata en función de su dirección de movimiento
    {
        // Cambia la definicion de la direccion de la rata
        derecha = !derecha;

        // Multiplica la escala de la rata jugador por -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    void Update()
    {
        if (cycles == Velocidad)//Llama la función correr si el movimiento previo ya ha sido completado
        {
            random = Random.insideUnitCircle;
            random = random.normalized;
            cycles = 0;
            Run(random);
            if (random.x > 0 && derecha)
            {
                // Voltea a la rata
                Flip();
            }
            // Checkea si la rata se mueve hacia la izquierda pero su sprite esta hacia la derecha
            else if (random.x < 0 && !derecha)
            {
                //Voltea a la rata
                Flip();
            }
        }
        else
        {
            cycles++;   
        }
        anim.SetFloat("MSpeed", Mathf.Abs(random.magnitude));// Define la magnitud de la velocidad para el animador de la rata
    }
    
}
