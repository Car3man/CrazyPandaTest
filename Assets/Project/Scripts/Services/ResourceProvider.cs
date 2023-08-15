using RedPanda.Project.Services.Interfaces;
using UnityEngine;

namespace RedPanda.Project.Services
{
    public class ResourceProvider : IResourceProvider
    {
        public Sprite GetIcon(string icon)
        {
            return Resources.Load<Sprite>($"Icons/{icon}");
        }
    }
}