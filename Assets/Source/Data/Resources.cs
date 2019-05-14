using UnityEngine;
using Views;
using Views.Enemy;

[CreateAssetMenu(fileName = "Resources", menuName = "Create/Resources")]
public class Resources : ScriptableObject
{
    public EnemyView[] EnemyPrefabs;
    public InfrastructureView[] InfrastructurePrefabs;
}