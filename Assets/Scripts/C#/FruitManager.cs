using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EvolvingCode
{
    public class FruitManager : MonoBehaviour
    {
        [SerializeField] private List<Fruit> fruitbasket;
        // Start is called before the first frame update
        void Start()
        {
            Fruit a = new Banana("Banana", 10, new Vector2());
            fruitbasket = new List<Fruit>();
            fruitbasket.Add(new Apple("", 5, 2));
            fruitbasket.Add(new Banana("", 5, new Vector2(2, 1)));
            fruitbasket.Add(new Fruit("Bom", 3));

            fruitbasket.ForEach(n => n.show_Fruit());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    [System.Serializable]
    public class Fruit
    {
        [SerializeField] private string fruit_Name;
        [SerializeField] private int amount;

        public string Fruit_Name { get => fruit_Name; set => fruit_Name = value; }
        public int Amount { get => amount; set => amount = value; }

        public Fruit(string pfruitName, int pamount)
        {
            Fruit_Name = pfruitName;
            Amount = pamount;
        }
        public virtual void show_Fruit()
        {
            Debug.Log(fruit_Name + " amount: " + amount.ToString());
        }
    }

    [System.Serializable]
    public class Apple : Fruit
    {
        [SerializeField] private int cores;

        public Apple(string pfruitName, int pamount, int pcores) : base(pfruitName, pamount)
        {
            Fruit_Name = "Apple";
            Amount = pamount;
            Cores = pcores;
        }

        public int Cores { get => cores; set => cores = value; }



        public override void show_Fruit()
        {
            Debug.Log(Fruit_Name
            + ", amount: " + Amount.ToString()
            + ", cores: " + Cores.ToString());
        }
    }

    [System.Serializable]
    public class Banana : Fruit
    {
        [SerializeField] private Vector2 position;

        public Vector2 Position { get => position; set => position = value; }

        public Banana(string pfruitName, int pamount, Vector2 ppos) : base(pfruitName, pamount)
        {
            Fruit_Name = "Banana";
            Amount = pamount;
            Position = ppos;
        }
        public override void show_Fruit()
        {
            Debug.Log(Fruit_Name
            + ", amount: " + Amount.ToString()
            + ", position: " + Position.ToString());
        }
    }
}
