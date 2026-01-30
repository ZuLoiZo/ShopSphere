import { Link } from 'react-router-dom'
import { ShoppingCart, User, LogOut } from 'lucide-react'
import { useSelector, useDispatch } from 'react-redux'
import { RootState } from '../../store/store'
import { logout } from '../../store/slices/authSlice'

export default function Header() {
  const dispatch = useDispatch()
  const { isAuthenticated, user } = useSelector((state: RootState) => state.auth)
  const { items } = useSelector((state: RootState) => state.cart)

  const handleLogout = () => {
    dispatch(logout())
  }

  return (
    <header className="bg-white shadow-sm sticky top-0 z-50">
      <nav className="container mx-auto px-4 py-4">
        <div className="flex items-center justify-between">
          {/* Logo */}
          <Link to="/" className="text-2xl font-bold text-primary-600">
            ShopSphere
          </Link>

          {/* Navigation */}
          <div className="hidden md:flex items-center space-x-8">
            <Link to="/" className="text-gray-700 hover:text-primary-600 transition-colors">
              Accueil
            </Link>
            <Link to="/products" className="text-gray-700 hover:text-primary-600 transition-colors">
              Produits
            </Link>
          </div>

          {/* Right side */}
          <div className="flex items-center space-x-4">
            {/* Cart */}
            <Link to="/cart" className="relative p-2 text-gray-700 hover:text-primary-600 transition-colors">
              <ShoppingCart size={24} />
              {items.length > 0 && (
                <span className="absolute -top-1 -right-1 bg-red-500 text-white text-xs font-bold rounded-full w-5 h-5 flex items-center justify-center">
                  {items.length}
                </span>
              )}
            </Link>

            {/* User menu */}
            {isAuthenticated ? (
              <div className="flex items-center space-x-2">
                <Link to="/profile" className="flex items-center space-x-2 text-gray-700 hover:text-primary-600 transition-colors">
                  <User size={20} />
                  <span className="hidden md:inline">{user?.firstName}</span>
                </Link>
                <button
                  onClick={handleLogout}
                  className="p-2 text-gray-700 hover:text-red-600 transition-colors"
                  title="Déconnexion"
                >
                  <LogOut size={20} />
                </button>
              </div>
            ) : (
              <div className="flex items-center space-x-2">
                <Link to="/login" className="px-4 py-2 text-gray-700 hover:text-primary-600 transition-colors">
                  Connexion
                </Link>
                <Link to="/register" className="px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700 transition-colors">
                  Inscription
                </Link>
              </div>
            )}
          </div>
        </div>
      </nav>
    </header>
  )
}
