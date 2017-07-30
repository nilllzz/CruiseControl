using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using static Core;

namespace CruiseControl.Content
{
    internal class Resources : IGameComponent
    {
        private Dictionary<string, Texture2D> _textures;

        public void Initialize()
        {
            _textures = new Dictionary<string, Texture2D>();
        }

        private static string GetResourcePath(string resourcePath)
        {
            return Path.Combine(Environment.CurrentDirectory, Controller.Content.RootDirectory, resourcePath);
        }

        internal Texture2D LoadTexture(string texturePath)
        {
            lock (_textures)
            {
                if (!_textures.TryGetValue(texturePath, out Texture2D texture))
                {
                    var path = GetResourcePath(texturePath);

                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        texture = Texture2D.FromStream(Controller.GraphicsDevice, stream);
                        texture.Name = texturePath;
                        _textures.Add(texturePath, texture);
                    }
                }

                return texture;
            }
        }
    }
}
