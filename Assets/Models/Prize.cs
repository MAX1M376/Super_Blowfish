using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Prize
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Sprite Image { get; set; }
}

public class PrizeString
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }

    public Prize ToPrize()
    {
        Sprite image;

        try
        {
            image = Resources.Load<Sprite>(Image);
        }
        catch (Exception)
        {
            image = null;
        }

        return new Prize
        {
            Name = Name,
            Description = Description,
            Image = image
        };
    }

    public Prize ToPrize(Sprite defaultSprite)
    {
        Sprite image;

        try
        {
            image = Resources.Load<Sprite>(Image);
        }
        catch (Exception)
        {
            image = defaultSprite;
        }

        return new Prize
        {
            Name = Name,
            Description = Description,
            Image = image
        };
    }
}
