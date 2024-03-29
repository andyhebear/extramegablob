/*  This file is produced by the C++/CLI AutoWrapper utility.
        Copyright (c) 2006 by Argiris Kirtzidis  */

#pragma once

#include "OgreResource.h"
#include "MogreStringInterface.h"
#include "MogrePrerequisites.h"
#include "MogreCommon.h"

namespace Mogre
{
	typedef Ogre::ResourceHandle ResourceHandle;
	
	interface class IResource_Listener_Receiver
	{
		void BackgroundLoadingComplete( Mogre::Resource^ param1 );
	
		void BackgroundPreparingComplete( Mogre::Resource^ param1 );
	
		void LoadingComplete( Mogre::Resource^ param1 );
	
		void PreparingComplete( Mogre::Resource^ param1 );
	
		void UnloadingComplete( Mogre::Resource^ param1 );
	
	};
	
	//################################################################
	//Listener
	//################################################################
	
	class Resource_Listener_Director : public Ogre::Resource::Listener
	{
		//Nested Types
	
		//Private Declarations
	private:
		gcroot<IResource_Listener_Receiver^> _receiver;
	
		//Internal Declarations
	
		//Public Declarations
	public:
		Resource_Listener_Director( IResource_Listener_Receiver^ recv )
			: _receiver(recv), doCallForBackgroundLoadingComplete(false), doCallForBackgroundPreparingComplete(false), doCallForLoadingComplete(false), doCallForPreparingComplete(false), doCallForUnloadingComplete(false)
		{
		}
	
		bool doCallForBackgroundLoadingComplete;
		bool doCallForBackgroundPreparingComplete;
		bool doCallForLoadingComplete;
		bool doCallForPreparingComplete;
		bool doCallForUnloadingComplete;
	
		virtual void backgroundLoadingComplete( Ogre::Resource* param1 ) override;
	
		virtual void backgroundPreparingComplete( Ogre::Resource* param1 ) override;
	
		virtual void loadingComplete( Ogre::Resource* param1 ) override;
	
		virtual void preparingComplete( Ogre::Resource* param1 ) override;
	
		virtual void unloadingComplete( Ogre::Resource* param1 ) override;
	
	
		//Protected Declarations
	
	};
	
	//################################################################
	//Resource
	//################################################################
	
	public ref class Resource : public Wrapper, public IStringInterface, public IResource_Listener_Receiver
	{
		//Nested Types
		public: ref class Listener;
	
		public: enum class LoadingState
		{
			LOADSTATE_UNLOADED = Ogre::Resource::LOADSTATE_UNLOADED,
			LOADSTATE_LOADING = Ogre::Resource::LOADSTATE_LOADING,
			LOADSTATE_LOADED = Ogre::Resource::LOADSTATE_LOADED,
			LOADSTATE_UNLOADING = Ogre::Resource::LOADSTATE_UNLOADING,
			LOADSTATE_PREPARED = Ogre::Resource::LOADSTATE_PREPARED,
			LOADSTATE_PREPARING = Ogre::Resource::LOADSTATE_PREPARING
		};
	
		public: ref class Listener abstract sealed
		{
		public:
			delegate static void BackgroundLoadingCompleteHandler( Mogre::Resource^ param1 );
			delegate static void BackgroundPreparingCompleteHandler( Mogre::Resource^ param1 );
			delegate static void LoadingCompleteHandler( Mogre::Resource^ param1 );
			delegate static void PreparingCompleteHandler( Mogre::Resource^ param1 );
			delegate static void UnloadingCompleteHandler( Mogre::Resource^ param1 );
		};
	
		//Private Declarations
	private protected:
		
		//Event and Listener fields
		Resource_Listener_Director* _listener;
		Mogre::Resource::Listener::BackgroundLoadingCompleteHandler^ _backgroundLoadingComplete;
		Mogre::Resource::Listener::BackgroundPreparingCompleteHandler^ _backgroundPreparingComplete;
		Mogre::Resource::Listener::LoadingCompleteHandler^ _loadingComplete;
		Mogre::Resource::Listener::PreparingCompleteHandler^ _preparingComplete;
		Mogre::Resource::Listener::UnloadingCompleteHandler^ _unloadingComplete;
	
	
		//Internal Declarations
	public protected:
		Resource( CLRObject* obj ) : Wrapper(obj)
		{
		}
	
		~Resource()
		{
			if (_listener != 0)
			{
				if (_native != 0) static_cast<Ogre::Resource*>(_native)->removeListener(_listener);
				delete _listener; _listener = 0;
			}
		}
	
		virtual Ogre::StringInterface* _IStringInterface_GetNativePtr() = IStringInterface::_GetNativePtr;
	
	
		//Public Declarations
	public:
	
	
		event Mogre::Resource::Listener::BackgroundLoadingCompleteHandler^ BackgroundLoadingComplete
		{
			void add(Mogre::Resource::Listener::BackgroundLoadingCompleteHandler^ hnd)
			{
				if (_backgroundLoadingComplete == CLR_NULL)
				{
					if (_listener == 0)
					{
						_listener = new Resource_Listener_Director(this);
						static_cast<Ogre::Resource*>(_native)->addListener(_listener);
					}
					_listener->doCallForBackgroundLoadingComplete = true;
				}
				_backgroundLoadingComplete += hnd;
			}
			void remove(Mogre::Resource::Listener::BackgroundLoadingCompleteHandler^ hnd)
			{
				_backgroundLoadingComplete -= hnd;
				if (_backgroundLoadingComplete == CLR_NULL) _listener->doCallForBackgroundLoadingComplete = false;
			}
		private:
			void raise( Mogre::Resource^ param1 )
			{
				if (_backgroundLoadingComplete)
					_backgroundLoadingComplete->Invoke( param1 );
			}
		}
	
		event Mogre::Resource::Listener::BackgroundPreparingCompleteHandler^ BackgroundPreparingComplete
		{
			void add(Mogre::Resource::Listener::BackgroundPreparingCompleteHandler^ hnd)
			{
				if (_backgroundPreparingComplete == CLR_NULL)
				{
					if (_listener == 0)
					{
						_listener = new Resource_Listener_Director(this);
						static_cast<Ogre::Resource*>(_native)->addListener(_listener);
					}
					_listener->doCallForBackgroundPreparingComplete = true;
				}
				_backgroundPreparingComplete += hnd;
			}
			void remove(Mogre::Resource::Listener::BackgroundPreparingCompleteHandler^ hnd)
			{
				_backgroundPreparingComplete -= hnd;
				if (_backgroundPreparingComplete == CLR_NULL) _listener->doCallForBackgroundPreparingComplete = false;
			}
		private:
			void raise( Mogre::Resource^ param1 )
			{
				if (_backgroundPreparingComplete)
					_backgroundPreparingComplete->Invoke( param1 );
			}
		}
	
		event Mogre::Resource::Listener::LoadingCompleteHandler^ LoadingComplete
		{
			void add(Mogre::Resource::Listener::LoadingCompleteHandler^ hnd)
			{
				if (_loadingComplete == CLR_NULL)
				{
					if (_listener == 0)
					{
						_listener = new Resource_Listener_Director(this);
						static_cast<Ogre::Resource*>(_native)->addListener(_listener);
					}
					_listener->doCallForLoadingComplete = true;
				}
				_loadingComplete += hnd;
			}
			void remove(Mogre::Resource::Listener::LoadingCompleteHandler^ hnd)
			{
				_loadingComplete -= hnd;
				if (_loadingComplete == CLR_NULL) _listener->doCallForLoadingComplete = false;
			}
		private:
			void raise( Mogre::Resource^ param1 )
			{
				if (_loadingComplete)
					_loadingComplete->Invoke( param1 );
			}
		}
	
		event Mogre::Resource::Listener::PreparingCompleteHandler^ PreparingComplete
		{
			void add(Mogre::Resource::Listener::PreparingCompleteHandler^ hnd)
			{
				if (_preparingComplete == CLR_NULL)
				{
					if (_listener == 0)
					{
						_listener = new Resource_Listener_Director(this);
						static_cast<Ogre::Resource*>(_native)->addListener(_listener);
					}
					_listener->doCallForPreparingComplete = true;
				}
				_preparingComplete += hnd;
			}
			void remove(Mogre::Resource::Listener::PreparingCompleteHandler^ hnd)
			{
				_preparingComplete -= hnd;
				if (_preparingComplete == CLR_NULL) _listener->doCallForPreparingComplete = false;
			}
		private:
			void raise( Mogre::Resource^ param1 )
			{
				if (_preparingComplete)
					_preparingComplete->Invoke( param1 );
			}
		}
	
		event Mogre::Resource::Listener::UnloadingCompleteHandler^ UnloadingComplete
		{
			void add(Mogre::Resource::Listener::UnloadingCompleteHandler^ hnd)
			{
				if (_unloadingComplete == CLR_NULL)
				{
					if (_listener == 0)
					{
						_listener = new Resource_Listener_Director(this);
						static_cast<Ogre::Resource*>(_native)->addListener(_listener);
					}
					_listener->doCallForUnloadingComplete = true;
				}
				_unloadingComplete += hnd;
			}
			void remove(Mogre::Resource::Listener::UnloadingCompleteHandler^ hnd)
			{
				_unloadingComplete -= hnd;
				if (_unloadingComplete == CLR_NULL) _listener->doCallForUnloadingComplete = false;
			}
		private:
			void raise( Mogre::Resource^ param1 )
			{
				if (_unloadingComplete)
					_unloadingComplete->Invoke( param1 );
			}
		}
	
	
		property Mogre::ResourceManager^ Creator
		{
		public:
			Mogre::ResourceManager^ get();
		}
	
		property String^ Group
		{
		public:
			String^ get();
		}
	
		property Mogre::ResourceHandle Handle
		{
		public:
			Mogre::ResourceHandle get();
		}
	
		property bool IsBackgroundLoaded
		{
		public:
			bool get();
		}
	
		property bool IsLoaded
		{
		public:
			bool get();
		}
	
		property bool IsLoading
		{
		public:
			bool get();
		}
	
		property bool IsManuallyLoaded
		{
		public:
			bool get();
		}
	
		property bool IsPrepared
		{
		public:
			bool get();
		}
	
		property bool IsReloadable
		{
		public:
			bool get();
		}
	
		property String^ Name
		{
		public:
			String^ get();
		}
	
		property String^ Origin
		{
		public:
			String^ get();
		}
	
		property size_t Size
		{
		public:
			size_t get();
		}
	
		property size_t StateCount
		{
		public:
			size_t get();
		}
	
		void _Init_CLRObject( );
	
		void Prepare( bool backgroundThread );
		void Prepare( );
	
		void Load( bool backgroundThread );
		void Load( );
	
		void Reload( );
	
		void Unload( );
	
		void Touch( );
	
		Mogre::Resource::LoadingState GetLoadingState( );
	
		void SetBackgroundLoaded( bool bl );
	
		void EscalateLoading( );
	
		void ChangeGroupOwnership( String^ newGroup );
	
		void _notifyOrigin( String^ origin );
	
		void _dirtyState( );
	
		void _fireLoadingComplete( bool wasBackgroundLoaded );
	
		void _firePreparingComplete( bool wasBackgroundLoaded );
	
		void _fireUnloadingComplete( );
	
		//------------------------------------------------------------
		// Implementation for IStringInterface
		//------------------------------------------------------------
	
		property Mogre::ParamDictionary_NativePtr ParamDictionary
		{
		public:
			Mogre::ParamDictionary_NativePtr get();
		}
	
	public:
		Mogre::Const_ParameterList^ GetParameters( );
	
	public:
		virtual bool SetParameter( String^ name, String^ value );
	
	public:
		virtual void SetParameterList( Mogre::Const_NameValuePairList^ paramList );
	
	public:
		virtual String^ GetParameter( String^ name );
	
	public:
		virtual void CopyParametersTo( Mogre::IStringInterface^ dest );
	
		DEFINE_MANAGED_NATIVE_CONVERSIONS( Resource )
	
		//Protected Declarations
	protected public:
		virtual void OnBackgroundLoadingComplete( Mogre::Resource^ param1 ) = IResource_Listener_Receiver::BackgroundLoadingComplete
		{
			BackgroundLoadingComplete( param1 );
		}
	
		virtual void OnBackgroundPreparingComplete( Mogre::Resource^ param1 ) = IResource_Listener_Receiver::BackgroundPreparingComplete
		{
			BackgroundPreparingComplete( param1 );
		}
	
		virtual void OnLoadingComplete( Mogre::Resource^ param1 ) = IResource_Listener_Receiver::LoadingComplete
		{
			LoadingComplete( param1 );
		}
	
		virtual void OnPreparingComplete( Mogre::Resource^ param1 ) = IResource_Listener_Receiver::PreparingComplete
		{
			PreparingComplete( param1 );
		}
	
		virtual void OnUnloadingComplete( Mogre::Resource^ param1 ) = IResource_Listener_Receiver::UnloadingComplete
		{
			UnloadingComplete( param1 );
		}
	
	
	
	};
	
	//################################################################
	//IManualResourceLoader
	//################################################################
	
	public interface class IManualResourceLoader
	{
		//Nested Types
	
		//Private Declarations
	
		//Internal Declarations
	
		//Public Declarations
		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_INTERFACE( IManualResourceLoader, Ogre::ManualResourceLoader )
	
		virtual Ogre::ManualResourceLoader* _GetNativePtr();
	
	public:
	
	
		virtual void PrepareResource( Mogre::Resource^ resource );
	
		virtual void LoadResource( Mogre::Resource^ resource );
	
	
		//Protected Declarations
	
	};
	
	//################################################################
	//IManualResourceLoader
	//################################################################
	
	public ref class ManualResourceLoader abstract : public Wrapper, public IManualResourceLoader
	{
		//Nested Types
	
		//Private Declarations
	private protected:
	
		//Internal Declarations
	public protected:
		ManualResourceLoader( CLRObject* obj ) : Wrapper(obj)
		{
		}
	
		virtual Ogre::ManualResourceLoader* _IManualResourceLoader_GetNativePtr() = IManualResourceLoader::_GetNativePtr;
	
	
		//Public Declarations
	public:
		ManualResourceLoader( );
	
	
		[Implementation::MethodIndex( 0 )]
		virtual void PrepareResource( Mogre::Resource^ resource );
	
		virtual void LoadResource( Mogre::Resource^ resource ) abstract;
	
	
		//Protected Declarations
	protected public:
	
	};
	
	
	public ref class ResourcePtr : public Resource
	{
	public protected:
			Ogre::ResourcePtr* _sharedPtr;
	
		ResourcePtr(Ogre::ResourcePtr& sharedPtr) : Resource( sharedPtr.getPointer() )
		{
			_sharedPtr = new Ogre::ResourcePtr(sharedPtr);
		}
	
		!ResourcePtr()
		{
			if (_sharedPtr != 0)
			{
				delete _sharedPtr;
				_sharedPtr = 0;
			}
		}
	
		~ResourcePtr()
		{
			this->!ResourcePtr();
		}
	
	public:
		DEFINE_MANAGED_NATIVE_CONVERSIONS_FOR_SHAREDPTR( ResourcePtr )
	
		ResourcePtr(Resource^ obj) : Resource( obj->_native )
		{
			_sharedPtr = new Ogre::ResourcePtr( static_cast<Ogre::Resource*>(obj->_native) );
		}
	
		virtual bool Equals(Object^ obj) override
		{
			ResourcePtr^ clr = dynamic_cast<ResourcePtr^>(obj);
			if (clr == CLR_NULL)
			{
				return false;
			}
	
			return (_native == clr->_native);
		}
		bool Equals(ResourcePtr^ obj)
		{
			if (obj == CLR_NULL)
			{
				return false;
			}
	
			return (_native == obj->_native);
		}
	
		static bool operator == (ResourcePtr^ val1, ResourcePtr^ val2)
		{
			if ((Object^)val1 == (Object^)val2) return true;
			if ((Object^)val1 == nullptr || (Object^)val2 == nullptr) return false;
			return (val1->_native == val2->_native);
		}
	
		static bool operator != (ResourcePtr^ val1, ResourcePtr^ val2)
		{
			return !(val1 == val2);
		}
	
		virtual int GetHashCode() override
		{
			return reinterpret_cast<int>( _native );
		}
	
		property IntPtr NativePtr
		{
			IntPtr get() { return (IntPtr)_sharedPtr; }
		}
	
		property bool Unique
		{
			bool get()
			{
				return (*_sharedPtr).unique();
			}
		}
	
		property int UseCount
		{
			int get()
			{
				return (*_sharedPtr).useCount();
			}
		}
	
		property Resource^ Target
		{
			Resource^ get()
			{
				return static_cast<Ogre::Resource*>(_native);
			}
		}
	};
	
	
	//################################################################
	//ManualResourceLoader_Proxy
	//################################################################
	
	class ManualResourceLoader_Proxy : public Ogre::ManualResourceLoader, public CLRObject
	{
	public:
		friend ref class Mogre::ManualResourceLoader;
	
		bool* _overriden;
	
		gcroot<Mogre::ManualResourceLoader^> _managed;
	
		virtual void _Init_CLRObject() override { *static_cast<CLRObject*>(this) = _managed; }
	
		ManualResourceLoader_Proxy( Mogre::ManualResourceLoader^ managedObj ) :
			Ogre::ManualResourceLoader( ),
			_managed(managedObj)
		{
		}
	
		virtual void prepareResource( Ogre::Resource* resource ) override;
	
		virtual void loadResource( Ogre::Resource* resource ) override;
	};
	

}
