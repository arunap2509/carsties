import type { Metadata } from 'next'
import './globals.css'
import Navbar from './nav/Navbar'
import ToasterProvide from './providers/ToasterProvide'
import { getCurrentUser } from './actions/authActions'
import SignalRProvider from './providers/SignalRProvider'

export const metadata: Metadata = {
  title: 'Create Next App',
  description: 'Generated by create next app',
}

export default async function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  const user = await getCurrentUser();

  return (
    <html lang="en">
      <body>
        <ToasterProvide />
        <Navbar />
        <main className='container mx-auto pt-10'>
          <SignalRProvider user={user}>
            {children}
          </SignalRProvider>
        </main>
      </body>
    </html>
  )
}
