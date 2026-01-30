import { Link } from 'react-router-dom'
import { ShoppingBag, Truck, CreditCard, Shield } from 'lucide-react'

export default function HomePage() {
  return (
    <div className="min-h-screen">
      {/* Hero Section */}
      <section className="bg-gradient-to-r from-primary-600 to-primary-800 text-white py-20">
        <div className="container mx-auto px-4 text-center">
          <h1 className="text-5xl font-bold mb-6">
            Bienvenue sur ShopSphere
          </h1>
          <p className="text-xl mb-8 max-w-2xl mx-auto">
            Découvrez notre sélection de produits de qualité à des prix imbattables
          </p>
          <Link to="/products" className="inline-block px-8 py-4 bg-white text-primary-600 font-semibold rounded-lg hover:bg-gray-100 transition-colors">
            Découvrir nos produits
          </Link>
        </div>
      </section>

      {/* Features */}
      <section className="py-16">
        <div className="container mx-auto px-4">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
            <div className="text-center">
              <div className="inline-flex items-center justify-center w-16 h-16 bg-primary-100 text-primary-600 rounded-full mb-4">
                <ShoppingBag size={32} />
              </div>
              <h3 className="text-xl font-semibold mb-2">Large sélection</h3>
              <p className="text-gray-600">Des milliers de produits disponibles</p>
            </div>
            <div className="text-center">
              <div className="inline-flex items-center justify-center w-16 h-16 bg-primary-100 text-primary-600 rounded-full mb-4">
                <Truck size={32} />
              </div>
              <h3 className="text-xl font-semibold mb-2">Livraison rapide</h3>
              <p className="text-gray-600">Livraison en 24-48h</p>
            </div>
            <div className="text-center">
              <div className="inline-flex items-center justify-center w-16 h-16 bg-primary-100 text-primary-600 rounded-full mb-4">
                <CreditCard size={32} />
              </div>
              <h3 className="text-xl font-semibold mb-2">Paiement sécurisé</h3>
              <p className="text-gray-600">Via Stripe</p>
            </div>
            <div className="text-center">
              <div className="inline-flex items-center justify-center w-16 h-16 bg-primary-100 text-primary-600 rounded-full mb-4">
                <Shield size={32} />
              </div>
              <h3 className="text-xl font-semibold mb-2">Garantie qualité</h3>
              <p className="text-gray-600">Produits certifiés</p>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="bg-gray-100 py-16">
        <div className="container mx-auto px-4 text-center">
          <h2 className="text-3xl font-bold mb-6">Prêt à commencer vos achats ?</h2>
          <Link to="/register" className="inline-block px-8 py-4 bg-primary-600 text-white font-semibold rounded-lg hover:bg-primary-700 transition-colors">
            Créer un compte
          </Link>
        </div>
      </section>
    </div>
  )
}
