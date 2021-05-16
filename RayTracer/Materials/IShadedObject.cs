namespace RayTracer.Materials
{
    public interface IShadedObject
    {
        public PhongMaterial Material { get; set; } //Change to IMaterial if more Materials are present.
    }
}
