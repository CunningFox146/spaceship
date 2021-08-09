using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Asteroids.Util
{
    public static class SaveSystem
    {
        private static readonly string FileName =
            Application.installMode == ApplicationInstallMode.Editor ? "SaveDebug" : "SaveProd";
        private static readonly string FilePath =
            $"{Application.persistentDataPath}/{FileName}.bytes";

        public static void Save(SaveData data)
        {
            FileStream fs = new FileStream(FilePath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                formatter.Serialize(fs, data);
            }
            catch (SerializationException e)
            {
                Debug.LogError($"Failed to serialize save file: {e.Message}");
            }
            finally
            {
                fs.Close();
            }
        }

        public static SaveData Load()
        {
            if (!File.Exists(FilePath))
            {
                Debug.Log($"Save file does not exist: {FilePath}");
                return null;
            }

            FileStream fs = new FileStream(FilePath, FileMode.Open);
            fs.Position = 0;
            SaveData data = null;

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                data = formatter.Deserialize(fs) as SaveData;
            }
            catch (SerializationException e)
            {
                Debug.LogError($"Failed to deserialize save file: {e.Message}");
            }
            finally
            {
                fs.Close();
            }

            return data;
        }
    }
}