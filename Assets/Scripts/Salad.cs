using System;
using System.Collections.Generic;

[Serializable]
public class Vegetable
{
    public string name;
    public float weight;

    public Vegetable(string name, float weight = 10)
    {
        this.name = name;
        this.weight = weight;
    }
}

[Serializable]
public class Salad
{
    private List<Vegetable> _vegetables;

    public bool isPickedUp { get; set; }
    public bool isEmpty { get { return _vegetables.Count == 0; } }

    public int vegetabeCount { get { return _vegetables.Count; } }

    public Salad()
    {
        _vegetables = new List<Vegetable>();
    }

    public Salad(List<Vegetable> vegetables)
    {
        this._vegetables = new List<Vegetable>(vegetables);
    }

    public void Add(Vegetable vegetable)
    {
        _vegetables.Add(vegetable);
    }

    public void Clear()
    {
        _vegetables.Clear();
    }

    public static bool operator ==(Salad salad1, Salad salad2)
    {
        if (salad1._vegetables.Count != salad2._vegetables.Count)
            return false;
        for (int i = 0; i < 2; i++)
        {
            if (salad1._vegetables[i].name != salad2._vegetables[i].name)
                return false;
        }

        return true;
    }

    public static bool operator !=(Salad salad1, Salad salad2)
    {
        if (salad1._vegetables.Count != salad2._vegetables.Count)
            return true;
        for (int i = 0; i < 2; i++)
        {
            if (salad1._vegetables[i].name != salad2._vegetables[i].name)
                return true;
        }

        return false;
    }

   
    public static Salad GenerateRandom(int typeOfVegetables = 3, int noOfVegetables = 3)
    {
        List<Vegetable> vegetables = new List<Vegetable>();
        int randomNumber = UnityEngine.Random.Range(2, noOfVegetables + 1);

        for (int i = 0; i < randomNumber; i++)
        {
            int randomVegetable = UnityEngine.Random.Range(1, typeOfVegetables + 1);
            switch (randomVegetable)
            {
                case 1:
                    vegetables.Add(new Vegetable(Const.VEG_A));
                    break;
                case 2:
                    vegetables.Add(new Vegetable(Const.VEG_B));
                    break;
                case 3:
                    vegetables.Add(new Vegetable(Const.VEG_C));
                    break;
                default:
                    break;
            }
        }

        return new Salad(vegetables);
    }

    public override string ToString()
    {
        string salad = "S[";
        foreach (var vegetable in _vegetables)
            salad += vegetable.name + ",";

        salad = salad.Remove(salad.Length - 1);
        salad += "]";
        return salad;
    }
}
