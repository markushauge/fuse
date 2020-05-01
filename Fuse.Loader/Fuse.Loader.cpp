#define DLLEXPORT extern "C" __declspec(dllexport)

using Fuse::Entry;

DLLEXPORT void Load()
{
	Entry::Load();
}

DLLEXPORT void Unload()
{
	Entry::Unload();
}
