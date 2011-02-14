/*
--------------------------------------------------------------------------------
This source file is part of MHydrax.
Visit ---

Copyright (C) 2009 Christian Haettig

This program is free software; you can redistribute it and/or modify it under
the terms of the GNU Lesser General Public License as published by the Free Software
Foundation; either version 3 of the License, or (at your option) any later
version.

This program is distributed in the hope that it will be useful, but WITHOUT
ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with
this program; if not, write to the Free Software Foundation, Inc., 59 Temple
Place - Suite 330, Boston, MA 02111-1307, USA, or go to
http://www.gnu.org/copyleft/lesser.txt.
--------------------------------------------------------------------------------
*/

// Util.h

#pragma once

using namespace System::Runtime::InteropServices;

namespace MHydrax {

	__inline std::string GetUnmanagedString(System::String^ str) {
		//get a pointer to an array of ANSI chars
		char* chars = (char*) Marshal::StringToHGlobalAnsi(str).ToPointer();
		//assign the array to an STL string
		std::string stl = chars;
		//free the memory used by the array
		//since the array is not managed, it will not be claimed by the garbage collector
		Marshal::FreeHGlobal((System::IntPtr)chars);
		// return STL string.
		return stl;
	}

	__inline System::String^ GetManagedString(std::string unmanagedstr) {
		return gcnew System::String(const_cast<char*>(unmanagedstr.c_str()));
	}

	__inline Mogre::Vector4 GetManagedVector4(Ogre::Vector4 v) {
		Mogre::Vector4 v2;
		v2.w = v.w;
		v2.x = v.x;
		v2.y = v.y;
		v2.z = v.z;
		return v2;
	}

	__inline Mogre::Vector3 GetManagedVector3(Ogre::Vector3 v) {
		Mogre::Vector3 v2;
		v2.x = v.x;
		v2.y = v.y;
		v2.z = v.z;
		return v2;
	}

	__inline Mogre::Vector2 GetManagedVector2(Ogre::Vector2 v) {
		Mogre::Vector2 v2;
		v2.x = v.x;
		v2.y = v.y;
		return v2;
	}
}
