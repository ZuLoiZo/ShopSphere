import { Link } from 'react-router-dom'
import { ShoppingCart } from 'lucide-react'
import { Product } from '../../services/productService'

interface ProductCardProps {
  product: Product
}

export default function ProductCard({ product }: ProductCardProps) {
  return (
    <div className="card group">
      <Link to={`/products/${product.id}`}>
        <div className="relative overflow-hidden rounded-lg mb-4">
          <img 
            src={product.imageUrl} 
            alt={product.name}
            className="w-full h-64 object-cover group-hover:scale-110 transition-transform duration-300"
          />
          {product.stock < 10 && product.stock > 0 && (
            <div className="absolute top-2 right-2 bg-orange-500 text-white px-2 py-1 rounded text-sm">
              Plus que {product.stock} en stock
            </div>
          )}
          {product.stock === 0 && (
            <div className="absolute top-2 right-2 bg-red-500 text-white px-2 py-1 rounded text-sm">
              Rupture de stock
            </div>
          )}
        </div>
      </Link>
      
      <div className="flex flex-col h-full">
        <div className="mb-2">
          <span className="text-xs text-gray-500">{product.categoryName}</span>
        </div>
        
        <Link to={`/products/${product.id}`}>
          <h3 className="text-lg font-semibold mb-2 hover:text-primary-600 transition-colors">
            {product.name}
          </h3>
        </Link>
        
        <p className="text-gray-600 text-sm mb-4 line-clamp-2 flex-grow">
          {product.description}
        </p>
        
        <div className="flex items-center justify-between mt-auto">
          <span className="text-2xl font-bold text-primary-600">
            {product.price.toFixed(2)} €
          </span>
          
          <button
            className="flex items-center gap-2 px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={product.stock === 0}
          >
            <ShoppingCart size={20} />
            <span>Ajouter</span>
          </button>
        </div>
      </div>
    </div>
  )
}
