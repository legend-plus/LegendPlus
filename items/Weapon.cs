using MiscUtil.Conversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendItems
{
    class Weapon : Item
    {
        String weaponClass;
        double damage;
        String damageType;

        BaseWeapon baseWeapon;

        public Weapon(BaseWeapon baseItem, String itemSprite = null, String itemName = null, String itemDescription = null, String itemType = null, String weaponClass = null, double damage = Double.NaN, String damageType = null) : base(baseItem, itemSprite, itemName, itemDescription, itemType)
        {
            this.baseWeapon = baseItem;
            this.weaponClass = weaponClass;
            this.damage = damage;
            this.damageType = damageType;
        }

        public String GetWeaponClass()
        {
            return weaponClass ?? baseWeapon.weaponClass;
        }

        public double GetDamage()
        {
            return Double.IsNaN(damage) ? baseWeapon.damage : damage;
        }

        public String GetDamageType()
        {
            return damageType ?? baseWeapon.damageType;
        }

        public bool HasWeaponClass()
        {
            return weaponClass != null;
        }

        public bool HasDamage()
        {
            return damage != Double.NaN;
        }

        public bool HasDamageType()
        {
            return damageType != null;
        }

        public override Item GetStatic()
        {
            return new Weapon(null, GetSprite(), GetName(), GetDescription(), GetItemType(), GetWeaponClass(), GetDamage(), GetDamageType());
        }

        public override byte[] Encode(BigEndianBitConverter converter)
        {
            byte[] spriteData = System.Text.Encoding.UTF8.GetBytes(GetSprite());
            byte[] nameData = System.Text.Encoding.UTF8.GetBytes(GetName());
            byte[] descriptionData = System.Text.Encoding.UTF8.GetBytes(GetDescription());
            byte[] typeData = System.Text.Encoding.UTF8.GetBytes(GetItemType());
            byte[] weaponClassData = System.Text.Encoding.UTF8.GetBytes(GetWeaponClass());
            byte[] damageData = converter.GetBytes(GetDamage());
            byte[] damageTypeData = System.Text.Encoding.UTF8.GetBytes(GetDamageType());

            byte[] spriteLength = converter.GetBytes(spriteData.Length);
            byte[] nameLength = converter.GetBytes(nameData.Length);
            byte[] descriptionLength = converter.GetBytes(descriptionData.Length);
            byte[] typeLength = converter.GetBytes(typeData.Length);
            byte[] weaponClassLength = converter.GetBytes(weaponClassData.Length);
            byte[] damageTypeLength = converter.GetBytes(damageTypeData.Length);

            byte[] output = new byte[32 + spriteData.Length + nameData.Length + descriptionData.Length + typeData.Length + weaponClassData.Length + damageTypeData.Length];

            System.Buffer.BlockCopy(spriteLength, 0, output, 0, 4);
            System.Buffer.BlockCopy(spriteData, 0, output, 4, spriteData.Length);

            System.Buffer.BlockCopy(nameLength, 0, output, 4 + spriteData.Length, 4);
            System.Buffer.BlockCopy(nameData, 0, output, 8 + spriteData.Length, nameData.Length);

            System.Buffer.BlockCopy(descriptionLength, 0, output, 8 + spriteData.Length + nameData.Length, 4);
            System.Buffer.BlockCopy(descriptionData, 0, output, 12 + spriteData.Length + nameData.Length, descriptionData.Length);

            System.Buffer.BlockCopy(typeLength, 0, output, 12 + spriteData.Length + nameData.Length + descriptionData.Length, 4);
            System.Buffer.BlockCopy(typeData, 0, output, 16 + spriteData.Length + nameData.Length + descriptionData.Length, typeData.Length);

            System.Buffer.BlockCopy(weaponClassLength, 0, output, 16 + spriteData.Length + nameData.Length + descriptionData.Length + typeData.Length, 4);
            System.Buffer.BlockCopy(weaponClassData, 0, output, 20 + spriteData.Length + nameData.Length + descriptionData.Length + typeData.Length, weaponClassData.Length);

            System.Buffer.BlockCopy(damageData, 0, output, 20 + spriteData.Length + nameData.Length + descriptionData.Length + typeData.Length + weaponClassData.Length, 8);

            System.Buffer.BlockCopy(damageTypeLength, 0, output, 28 + spriteData.Length + nameData.Length + descriptionData.Length + typeData.Length + weaponClassData.Length, 4);
            System.Buffer.BlockCopy(damageTypeData, 0, output, 32 + spriteData.Length + nameData.Length + descriptionData.Length + typeData.Length + weaponClassData.Length, damageTypeData.Length);

            return output;
        }
    }
}
