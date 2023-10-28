import { getDetailedViewData } from '@/app/actions/auctionActions'
import Heading from '@/app/components/Heading';
import React from 'react'
import CountdownTimer from '../../CountdownTimer';
import CarImage from '../../CarImage';
import DetailedSpecs from './DetailedSpec';
import { getCurrentUser } from '@/app/actions/authActions';
import EditButton from './EditButton';
import DeleteButton from './DeleteButton';

export default async function Details({ params }: { params: { id: string } }) {
    const data = await getDetailedViewData(params.id);
    const user = await getCurrentUser();
    return (
        <div>
            <div className='flex justify-between'>
                <div className='flex items-center gap-3'>
                    <Heading title={`${data[0].make} ${data[0].model}`} subtitle='' center={false} />
                    {user?.username === data[0].seller && (
                        <EditButton id={data[0].id} />
                    )}
                    {user?.username === data[0].seller && (
                        <DeleteButton id={data[0].id} />
                    )}
                </div>
                <div className='flex gap-3'>
                    <h3 className='text-2xl font-semibold'>Time Remaining:</h3>
                    <CountdownTimer auctionEnd={data[0].auctionEnd} />
                </div>
            </div>

            <div className='grid grid-cols-2 gap-6 mt-3'>
                <div className='w-full bg-gray-200 aspect-h-10 aspect-w-16 rounded-lg overflow-hidden'>
                    <CarImage imageUrl={data[0].imageUrl} />
                </div>
                <div className='border-2 rounded-lg p-2 bg-gray-100'>
                    <Heading title='Bids' subtitle='' center={false} />
                </div>
            </div>

            <div className='mt-3 grid grid-cols-1 rounded-lg'>
                <DetailedSpecs auction={data[0]} />
            </div>
        </div>
    )
}
