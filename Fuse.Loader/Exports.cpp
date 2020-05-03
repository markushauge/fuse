#define DLLEXPORT extern "C" __declspec(dllexport)

using namespace Fuse;

DLLEXPORT void Load()
{
    Entry::Load();
}

DLLEXPORT void Unload()
{
    Entry::Unload();
}
