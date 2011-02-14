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

// MHydrax.cpp
// This is the main DLL file.

#include "Stdafx.h"

#include "Util.h"

#include "MEnums.h"
#include "MHelp.h"
#include "MMesh.h"
#include "MDecalsManager.h"
#include "MMaterialManager.h"
#include "MCfgFileManager.h"
#include "MNoise.h"
#include "MPerlin.h"
#include "MFFT.h"
#include "MGodRaysManager.h"
#include "MModule.h"
#include "MHydrax.h"
#include "MProjectedGrid.h"
#include "MSimpleGrid.h"
#include "MRadialGrid.h"