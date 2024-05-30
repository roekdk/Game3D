using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;    
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }
    public void OnInteract()
    { 
        CharacterManager.Instance.Player.itemData = data;
        
        if(data.type == ItemType.Consumable && !CharacterManager.Instance.Player.condition.isBuff)
        {
            for(int i = 0; i < data.consumables.Length; i++)
            {
                switch (data.consumables[i].type)
                {
                    case ConsumableType.Health:
                        CharacterManager.Instance.Player.condition.Heal(data.consumables[i].value); break;
                    case ConsumableType.Hunger:
                        CharacterManager.Instance.Player.condition.Eat(data.consumables[i].value);break;                    
                    case ConsumableType.BuffTime:
                        CharacterManager.Instance.Player.condition.buffCheck((int)data.consumables[i+1].value,data.consumables[i].value);break;
                }
            }
            Destroy(gameObject);
        }
        if(data.type == ItemType.Resource)
        {
            for(int i = 0; i < data.consumables.Length; i++)
            {
                switch (data.consumables[i].type)
                    {
                        case ConsumableType.Health:
                            CharacterManager.Instance.Player.condition.Heal(data.consumables[i].value); break;
                        case ConsumableType.Hunger:
                            CharacterManager.Instance.Player.condition.Eat(data.consumables[i].value);break;
                    }
            }
            Destroy(gameObject);
        }
        //CharacterManager.Instance.Player.addItem?.Invoke();        
    }
}
