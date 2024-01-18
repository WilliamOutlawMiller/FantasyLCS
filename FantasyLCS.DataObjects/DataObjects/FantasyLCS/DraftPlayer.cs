using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DraftPlayer
{
    public int ID { get; set; }

    public string Name { get; set; }

    public bool Drafted { get; set; }

    public Position Position { get; set; }

    // Foreign Key for Draft
    public int DraftID { get; set; }

    // Navigation Property for Draft
    public Draft Draft { get; set; }

    public string ImagePath 
    { 
        get
        {
            return "/headshots/" + Name + ".webp";
        }
    }
}
public enum Position
{
    Top,
    Jungle,
    Mid,
    Bot,
    Support
}