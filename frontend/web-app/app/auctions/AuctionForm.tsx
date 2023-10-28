'use client'

import { Button, TextInput } from 'flowbite-react';
import React, { useEffect } from 'react'
import { FieldValues, useForm } from 'react-hook-form'
import Input from '../components/Input';
import DateInput from '../components/DateInput';
import { createAuctions, updateAuctions } from '../actions/auctionActions';
import { usePathname, useRouter } from 'next/navigation';
import { toast } from 'react-hot-toast';
import { Auction } from '@/types';

type Props = {
    auction?: Auction
}

export default function AuctionForm({ auction }: Props) {

    const { control,
        handleSubmit,
        setFocus,
        reset,
        formState: { isSubmitting, isValid }
    } = useForm({
        mode: 'onTouched'
    });
    const router = useRouter();
    const pathname = usePathname();

    useEffect(() => {
        if (auction) {
            const { make, model, mileage, year, color } = auction;
            reset({ make, model, mileage, year, color });
        }
        setFocus('make')
    }, [setFocus, auction, reset])

    const onSubmitting = async (data: FieldValues) => {
        try {
            let id = '';
            let res;
            if (pathname === '/auctions/create') {
                res = await createAuctions(data);
                id = res.id;
            } else {
                if (auction) {
                    res = await updateAuctions(data, auction.id);
                    id = auction.id;
                }
            }

            if (res.error) {
                throw res.error;
            }

            router.push(`/auctions/details/${id}`)
        } catch (e: any) {
            toast.error(e.status + ' ' + e.message);
        }
    }

    return (
        <form className='flex flex-col mt-3' onSubmit={handleSubmit(onSubmitting)}>
            <Input label='Make' name='make' control={control} rules={{ required: 'Make is required' }} />
            <Input label='Model' name='model' control={control} rules={{ required: 'Model is required' }} />
            <Input label='Color' name='color' control={control} rules={{ required: 'Color is required' }} />

            <div className='grid grid-cols-2 gap-3'>
                <Input label='Year' name='year' control={control} rules={{ required: 'Year is required' }} />
                <Input label='Mileage' type='number' name='mileage' control={control} rules={{ required: 'Mileage is required' }} />
            </div>

            {pathname === '/auctions/create' &&
                <>
                    <Input label='Image Url' name='imageUrl' control={control} rules={{ required: 'Image is required' }} />

                    <div className='grid grid-cols-2 gap-3'>
                        <Input label='Reserve Price (enter 0 if no reserve)' name='reservePrice' control={control} rules={{ required: 'Reserve Price is required' }} />
                        <DateInput
                            label='Auction End date/time'
                            name='auctionEnd'
                            control={control}
                            dateFormat='dd MMM yyyy h:mm a'
                            showTimeSelect
                            rules={{ required: 'Auction End is required' }} />
                    </div>
                </>
            }

            <div className='flex justify-between'>
                <Button outline color='gray'>Cancel</Button>
                <Button isProcessing={isSubmitting} disabled={!isValid} type='submit' outline color='success'>Submit</Button>
            </div>
        </form>
    )
}
